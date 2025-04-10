using Domain.Interfaces.Repositories;
using Domain.Records;
using MassTransit;

namespace Application.Messaging.Consumers;
public class UploadFileConsumer : IConsumer<UploadFileMessage>
{
    private readonly IAwsService _awsService;
    public UploadFileConsumer(IAwsService awsService)
    {
        _awsService = awsService;
    }

    public async Task Consume(ConsumeContext<UploadFileMessage> context)
    {
        var msg = context.Message;
        using var stream = File.OpenRead(msg.TempFilePath);
        await _awsService.UploadFileAsync(
            msg.Bucket,
            msg.Path,
            stream,
            msg.ContentType, context.CancellationToken
        );
        File.Delete(msg.TempFilePath);
    }
}
