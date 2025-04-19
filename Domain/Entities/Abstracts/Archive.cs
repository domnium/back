using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities.Abstracts;
public abstract class Archive : Entity
{
    public BigString? AwsKey { get; protected set; }
    public ContentType? ContentType { get; protected set; }
    public DateTime? UrlExpired { get; protected set; }
    public Url? UrlTemp { get; protected set; }
    public string? BucketName { get; protected set; }
    public BigString? TemporaryPath { get; protected set; }

    protected Archive() { }

    public Archive(BigString? awsKey, BigString temporaryPath)
    {
        AddNotificationsFromValueObjects(awsKey, temporaryPath);
        AwsKey = awsKey;
        TemporaryPath = temporaryPath;
    }

    public Archive(BigString temporaryPath)
    {
        AddNotificationsFromValueObjects(temporaryPath);
        TemporaryPath = temporaryPath;
    }


    public void SetTemporaryUrl(Url url, DateTime expiration)
    {
        AddNotificationsFromValueObjects(url);
        UrlTemp = url;
        UrlExpired = expiration;
    }

    public void SetBucket(string bucket)
    {
        BucketName = bucket;
    }
}
