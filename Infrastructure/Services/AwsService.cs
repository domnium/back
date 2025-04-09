using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.Runtime;
using Domain.Interfaces.Repositories;
using Domain;
using Amazon;

namespace Application.Services;

public sealed class AwsService : IAwsService
{
    private readonly IAmazonS3 _awsS3Client;

    public AwsService()
    {
        _awsS3Client = new AmazonS3Client(
            new BasicAWSCredentials(Configuration.AwsKeyId, Configuration.AwsKeySecret),
            new AmazonS3Config { RegionEndpoint = RegionEndpoint.GetBySystemName(Configuration.AwsRegion) }
        );
    }

    public async Task<string> UploadFileAsync(string bucketName, string key, Stream file, string contentType)
    {
        using var newMemoryStream = new MemoryStream();
        await file.CopyToAsync(newMemoryStream);

        var fileTransferUtility = new TransferUtility(_awsS3Client);
        await fileTransferUtility.UploadAsync(new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = key,
            BucketName = bucketName,
            ContentType = contentType
        });
        return key;
    }

    public async Task<bool> DeleteFileAsync(string bucketName, string key)
    {
        var response = await _awsS3Client.DeleteObjectAsync(bucketName, key);
        return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
    }

    public async Task<Stream> GetFileAsync(string bucketName, string key)
    {
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = key
        };

        using var response = await _awsS3Client.GetObjectAsync(request);
        var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task<string> GeneratePreSignedUrlAsync(string bucketName, double duration, string objectKey, string contentType)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = objectKey,
            Expires = DateTime.UtcNow.AddHours(duration),
            Verb = HttpVerb.GET,
            ResponseHeaderOverrides = new ResponseHeaderOverrides
            {
                ContentType = contentType
            }
        };

        return await Task.FromResult(_awsS3Client.GetPreSignedURL(request));
    }
}
