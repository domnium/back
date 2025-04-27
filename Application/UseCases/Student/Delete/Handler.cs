using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.Delete;

/// <summary>
/// Handler responsável pela exclusão lógica de um estudante e envio assíncrono da exclusão da imagem associada.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMessageQueueService _messageQueueService;

    /// <summary>
    /// Construtor do handler de deleção de estudante.
    /// </summary>
    public Handler(
        IStudentRepository studentRepository,
        IDbCommit dbCommit,
        IMessageQueueService messageQueueService)
    {
        _studentRepository = studentRepository;
        _dbCommit = dbCommit;
        _messageQueueService = messageQueueService;
    }

    /// <summary>
    /// Manipula a exclusão de um estudante e enfileira a remoção da imagem em armazenamento externo.
    /// </summary>
    /// <param name="request">Request com o ID do estudante</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse{object}"/> com status da operação</returns>
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca o estudante no repositório
        var studentFound = await _studentRepository.GetWithParametersAsyncWithTracking(
            x => x.Id == request.StudentId,
            cancellationToken,
            x => x.Picture,
            x => x.User,
            x => x.StudentLectures,
            x => x.StudentCourses,
            x => x.Subscriptions
        );

        // Verifica se o estudante foi encontrado
        if (studentFound is null)
        {
            return new BaseResponse<object>(
                statusCode: 404,
                message: "Student not found"
            );
        }

        // Remove o estudante do repositório
        _studentRepository.Delete(studentFound);

        // Lista de tarefas para exclusão de arquivos
        var deleteTasks = new List<Task>();

        // Enfileira a exclusão da imagem, se existir
        if (studentFound.Picture?.AwsKey is not null &&
            !string.IsNullOrWhiteSpace(studentFound.Picture.BucketName))
        {
            deleteTasks.Add(_messageQueueService.EnqueueDeleteMessageAsync(
                new DeleteFileMessage(
                    studentFound.Picture.BucketName,
                    studentFound.Picture.AwsKey.Body),
                cancellationToken
            ));
        }

        // Aguarda a conclusão das tarefas de exclusão
        await Task.WhenAll(deleteTasks);

        // Confirma as alterações no banco de dados
        await _dbCommit.Commit(cancellationToken);

        // Retorna sucesso
        return new BaseResponse<object>(
            statusCode: 200,
            message: "Student deleted successfully"
        );
    }
}