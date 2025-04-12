using System;
using Domain.Interfaces.Services;

namespace Infrastructure.Services;

public class TemporaryStorageService : ITemporaryStorageService
{
    public async Task<string> SaveAsync(string pathBase, Guid id, Stream file, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(pathBase))
            throw new ArgumentException("Path base is required.", nameof(pathBase));

        if (!Directory.Exists(pathBase))
            Directory.CreateDirectory(pathBase);

        // Define extensão padrão, pode ser ajustado depois se precisar
        var fullPath = Path.Combine(pathBase, $"_{id}.mp4");

        using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        await file.CopyToAsync(fileStream, cancellationToken);

        return fullPath;
    }
}