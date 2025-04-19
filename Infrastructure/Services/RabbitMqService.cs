using MassTransit;
using Domain.Interfaces.Services;
using Domain.Records;

namespace Infrastructure.Services;

public class RabbitMqService : IMessageQueueService
{
    private readonly IPublishEndpoint _publishEndpoint;
    public RabbitMqService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task EnqueueUploadMessageAsync(UploadFileMessage message, CancellationToken cancellationToken)
        => await _publishEndpoint.Publish(message, cancellationToken);
    public async Task EnqueueDeleteMessageAsync(DeleteFileMessage message, CancellationToken cancellationToken)
        => await _publishEndpoint.Publish(message, cancellationToken);
    public async Task EnqueueEmailMessageAsync(EmailMessage message, CancellationToken cancellationToken)
    => await _publishEndpoint.Publish(message, cancellationToken);
}