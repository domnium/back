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

    public async Task EnqueueUploadMessageAsync(UploadFileMessage Message, CancellationToken cancellationToken)
        => await _publishEndpoint.Publish(Message, cancellationToken);
    public async Task EnqueueDeleteMessageAsync(DeleteFileMessage Message, CancellationToken cancellationToken)
        => await _publishEndpoint.Publish(Message, cancellationToken);
    public async Task EnqueueEmailMessageAsync(EmailMessage Message, CancellationToken cancellationToken)
    => await _publishEndpoint.Publish(Message, cancellationToken);
}