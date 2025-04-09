using System;

namespace Domain.Interfaces.Repositories;

public interface IAwsRepository
{
    Task<string> UploadFileAsync(string bucketName, string key, Stream file);
    Task<bool> DeleteFileAsync(string bucket, string key);
    Task<Stream> GetFileAsync(string bucketName, string key);
    Task<string> GeneratePreSignedUrlAsync(string bucketname, double duration, string objectKey, string contentType);
}
