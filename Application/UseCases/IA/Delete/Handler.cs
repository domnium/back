using System;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.IA.Delete;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IIARepository _iaRepository;
    private readonly IDbCommit _dbCommit;   
    private readonly IMessageQueueService _messageQueueService;
    public Handler(IIARepository iaRepository, 
        IDbCommit dbCommit,
        IMessageQueueService messageQueueService)
    {
        _iaRepository = iaRepository;
        _dbCommit = dbCommit;
        _messageQueueService = messageQueueService;
    }
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var IA = await _iaRepository.GetWithParametersAsync(
                c => c.Id.Equals(request.Id), cancellationToken);

        if (IA is null)
            return new BaseResponse(404, "IA not found");

        if (IA.Picture?.AwsKey is not null && !string.IsNullOrWhiteSpace(IA.Picture.BucketName))
        {
            await _messageQueueService.EnqueueDeleteMessageAsync(
                    new DeleteFileMessage(IA.Picture.BucketName, IA.Picture.AwsKey!.Body),
                    cancellationToken);
        }

        _iaRepository.Delete(IA);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse(200, "IA deleted", null);
    }
}