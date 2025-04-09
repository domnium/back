using System;

namespace Domain.Interfaces.Repositories;

public interface IAwsService
{
    Task<string> UploadFileAsync(string bucketName, string key, Stream file, string contentType);
    Task<bool> DeleteFileAsync(string bucket, string key);
    Task<Stream> GetFileAsync(string bucketName, string key);
    Task<string> GeneratePreSignedUrlAsync(string bucketname, double duration, string objectKey, string contentType);
}
