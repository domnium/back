using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Teacher.Delete;

/// <summary>
/// Handler responsável pela exclusão lógica de um professor e envio assíncrono da exclusão da imagem associada.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMessageQueueService _messageQueueService;

    /// <summary>
    /// Construtor do handler de deleção de professor.
    /// </summary>
    public Handler(ITeacherRepository teacherRepository,
        IDbCommit dbCommit,
        IMessageQueueService messageQueueService)
    {
        _teacherRepository = teacherRepository;
        _dbCommit = dbCommit;
        _messageQueueService = messageQueueService;
    }

    /// <summary>
    /// Manipula a exclusão de um professor e enfileira a remoção da imagem em armazenamento externo.
    /// </summary>
    /// <param name="request">Request com o ID do professor</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status da operação</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var teacherFound = await _teacherRepository
            .GetWithParametersAsync(x => x.Id == request.TeacherId, cancellationToken);

        if (teacherFound is null)
            return new BaseResponse(404, "Teacher not found");

        await _teacherRepository.DeleteAsync(teacherFound, cancellationToken);

        // Enfileira a exclusão da imagem, se existir
        if (teacherFound.Picture?.AwsKey is not null &&
            !string.IsNullOrWhiteSpace(teacherFound.Picture.BucketName))
        {
            await _messageQueueService.EnqueueDeleteMessageAsync(
                new DeleteFileMessage(
                    teacherFound.Picture.BucketName,
                    teacherFound.Picture.AwsKey.Body),
                cancellationToken
            );
        }

        await _dbCommit.Commit(cancellationToken);

        return new BaseResponse(200, "Teacher deleted successfully");
    }
}
