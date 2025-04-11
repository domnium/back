using System;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Delete;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMessageQueueService _messageQueueService;

    public Handler(ICourseRepository courseRepository,
        IDbCommit dbCommit,
        IMessageQueueService messageQueueService)
    {
        _courseRepository = courseRepository;
        _dbCommit = dbCommit;
        _messageQueueService = messageQueueService;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository
            .GetWithParametersAsync(c => c.Id.Equals(request.id), cancellationToken);

        if (course is null)
            return new BaseResponse(404, "Course not found");

        await _courseRepository.DeleteAsync(course, cancellationToken);
        var deleteTasks = new List<Task>();

        if (course.Image?.AwsKey is not null && !string.IsNullOrWhiteSpace(course.Image.BucketName))
        {
            deleteTasks.Add(_messageQueueService.EnqueueDeleteMessageAsync(
                new DeleteFileMessage(course.Image.BucketName, course.Image.AwsKey.Body),
                cancellationToken
            ));
        }

        if (course.Trailer?.AwsKey is not null && !string.IsNullOrWhiteSpace(course.Trailer.BucketName))
        {
            deleteTasks.Add(_messageQueueService.EnqueueDeleteMessageAsync(
                new DeleteFileMessage(course.Trailer.BucketName, course.Trailer.AwsKey.Body),
                cancellationToken
            ));
        }
        await Task.WhenAll(deleteTasks); 
        await _dbCommit.Commit(cancellationToken); 
        return new BaseResponse(200, "Course deleted successfully");
    }
}

