using System;
using Domain.Records;

namespace Domain.Interfaces.Services;

public interface IMessageQueueService
{
    Task EnqueueUploadMessageAsync(UploadFileMessage Message, 
        CancellationToken cancellationToken);
    Task EnqueueDeleteMessageAsync(DeleteFileMessage Message,
        CancellationToken cancellationToken);

    Task EnqueueEmailMessageAsync(EmailMessage Message,
        CancellationToken cancellationToken);
}