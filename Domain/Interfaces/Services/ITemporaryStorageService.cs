using System;

namespace Domain.Interfaces.Services;

public interface ITemporaryStorageService
{
    Task<string> SaveAsync(string Path, Guid id, Stream file, CancellationToken cancellationToken);
}