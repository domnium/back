using System;
using Domain.Records;

namespace Domain.Interfaces.Services;

public interface IMessageQueueService
{
    Task EnqueueUploadMessageAsync(UploadFileMessage message, 
        CancellationToken cancellationToken);
}