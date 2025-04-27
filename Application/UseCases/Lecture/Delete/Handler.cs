using System;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Lecture.Delete;

/// <summary>
/// Handler responsável por deletar uma aula (lecture),
/// removendo-a do banco de dados e enfileirando a exclusão do vídeo correspondente do armazenamento externo.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly ILectureRepository _lectureRepository;
    private readonly IMessageQueueService _messageQueueService;
    private readonly IDbCommit _dbCommit;

    /// <summary>
    /// Construtor do handler de exclusão de Lecture.
    /// </summary>
    /// <param name="lectureRepository">Repositório de aulas</param>
    /// <param name="messageQueueService">Serviço de mensageria para upload/delete assíncronos</param>
    /// <param name="dbCommit">Serviço para commit da unidade de trabalho</param>
    public Handler(
        ILectureRepository lectureRepository,
        IMessageQueueService messageQueueService,
        IDbCommit dbCommit)
    {
        _lectureRepository = lectureRepository;
        _messageQueueService = messageQueueService;
        _dbCommit = dbCommit;
    }

    /// <summary>
    /// Executa a exclusão de uma aula e enfileira a exclusão do vídeo no armazenamento.
    /// </summary>
    /// <param name="request">Request com o ID da aula a ser removida</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Instância de <see cref="BaseResponse{object}"/> com status da operação</returns>
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca a aula no repositório
        var lecture = await _lectureRepository.GetWithParametersAsyncWithTracking(
            x => x.Id == request.id,
            cancellationToken,
            x => x.StudentLectures,
            x => x.Video
        );

        // Verifica se a aula foi encontrada
        if (lecture is null)
        {
            return new BaseResponse<object>(
                statusCode: 404,
                message: "Lecture not found"
            );
        }

        // Remove a aula do repositório
        _lectureRepository.Delete(lecture);

        // Confirma as alterações no banco de dados
        await _dbCommit.Commit(cancellationToken);

        // Enfileira a exclusão do vídeo, se existir
        if (lecture.Video?.AwsKey is not null && !string.IsNullOrWhiteSpace(lecture.Video.BucketName))
        {
            await _messageQueueService.EnqueueDeleteMessageAsync(
                new DeleteFileMessage(
                    lecture.Video.BucketName,
                    lecture.Video.AwsKey.Body
                ),
                cancellationToken
            );
        }

        // Retorna sucesso
        return new BaseResponse<object>(
            statusCode: 200,
            message: "Lecture deleted successfully"
        );
    }
}