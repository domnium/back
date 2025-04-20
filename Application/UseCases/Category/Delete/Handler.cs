using System;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.Delete;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDbCommit _dbCommit;   
    private readonly IMessageQueueService _messageQueueService;
    public Handler(ICategoryRepository categoryRepository, 
        IDbCommit dbCommit,
        IMessageQueueService messageQueueService)
    {
        _categoryRepository = categoryRepository;
        _dbCommit = dbCommit;
        _messageQueueService = messageQueueService;
    }
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetWithParametersAsync(
                c => c.Id.Equals(request.Id), cancellationToken);

        if (category is null)
            return new BaseResponse(404, "Category not found");

        _ = _messageQueueService.EnqueueDeleteMessageAsync(
                new DeleteFileMessage(category.Image.BucketName, category.Image.AwsKey.Body),
                cancellationToken);

        _categoryRepository.Delete(category);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse(200, "Category deleted", null, category);
    }
}