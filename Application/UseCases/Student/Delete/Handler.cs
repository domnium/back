using Domain;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.Delete;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IPictureRepository _pictureRepository;
    private readonly IMessageQueueService _messageQueueService;

    public Handler(IStudentRepository studentRepository,
     IDbCommit dbCommit,
     IPictureRepository pictureRepository,
     IMessageQueueService messageQueueService)
    {
        _studentRepository = studentRepository;
        _dbCommit = dbCommit;
        _pictureRepository = pictureRepository;
        _messageQueueService = messageQueueService;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var studentFound = await _studentRepository
            .GetWithParametersAsync(x => x.Id.Equals(request.StudentId),
             cancellationToken);

        if(studentFound is null) return new BaseResponse(400, "User does not exists");
        await _pictureRepository.DeleteAsync(studentFound.Picture, cancellationToken);
        await _studentRepository.DeleteAsync(studentFound, cancellationToken);

        //
        await _messageQueueService.EnqueueDeleteMessageAsync(new DeleteFileMessage(
            studentFound.Picture.BucketName,
            studentFound.Picture.AwsKey.Body), cancellationToken);
            
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse(200, "Student Deleted");
    }
}
