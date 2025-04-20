using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.Delete;

/// <summary>
/// Handler responsável pela exclusão lógica de um estudante e envio assíncrono da exclusão da imagem associada.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMessageQueueService _messageQueueService;

    /// <summary>
    /// Construtor do handler de deleção de estudante.
    /// </summary>
    public Handler(IStudentRepository studentRepository,
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
    /// <returns><see cref="BaseResponse"/> com status da operação</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var studentFound = await _studentRepository.GetWithParametersAsyncWithTracking(
            x => x.Id == request.StudentId,
            cancellationToken,
            x => x.Picture,
            x => x.User,
            x => x.StudentLectures,
            x => x.StudentCourses,
            x => x.Subscriptions
        );

        if (studentFound is null)
            return new BaseResponse(404, "Student not found");

        _studentRepository.Delete(studentFound);
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

        await Task.WhenAll(deleteTasks);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse(200, "Student deleted successfully");
    }
}
