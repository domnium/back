using System;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MassTransit;

namespace Application.Messaging.Consumers;

public class DeleteFileConsumer : IConsumer<DeleteFileMessage>
{
    private readonly IAwsService _awsService;
    public DeleteFileConsumer(IAwsService awsService)
    {
        _awsService = awsService;
    }

    public async Task Consume(ConsumeContext<DeleteFileMessage> context)
    {
        var msg = context.Message;
        await _awsService.DeleteFileAsync(msg.Bucket, msg.Path, 
            context.CancellationToken);
    }
}
