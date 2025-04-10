using System;

namespace Domain.Interfaces.Repositories;

public interface IAwsService
{
    Task<string> UploadFileAsync(string bucketName, string key, Stream file, string contentType, CancellationToken cancellationToken);
    Task<bool> DeleteFileAsync(string bucket, string key, CancellationToken cancellationToken);
    Task<Stream> GetFileAsync(string bucketName, string key, CancellationToken cancellationToken);
    Task<string> GeneratePreSignedUrlAsync(string bucketname, double duration, string objectKey, string contentType, CancellationToken cancellationToken);
}
