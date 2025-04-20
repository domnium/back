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
        var course = await _courseRepository.GetWithParametersAsyncWithTracking(
            c => c.Id == request.id,
            cancellationToken,
            c => c.Modules,
            c => c.Modules.Select(m => m.Lectures),
            c => c.Picture,
            c => c.Trailer,
            c => c.Parameters
        );

        if (course is null)
            return new BaseResponse(404, "Course not found");

        _courseRepository.Delete(course);
        var deleteTasks = new List<Task>();

        if (course.Picture?.AwsKey is not null && !string.IsNullOrWhiteSpace(course.Picture.BucketName))
        {
            deleteTasks.Add(_messageQueueService.EnqueueDeleteMessageAsync(
                new DeleteFileMessage(course.Picture.BucketName, course.Picture.AwsKey.Body),
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

