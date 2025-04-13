using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities.Abstracts;
public abstract class Archive : Entity
{
    public BigString AwsKey { get; private set; }
    public ContentType? ContentType { get; private set; }
    public DateTime? UrlExpired { get; private set; }
    public Url? UrlTemp { get; private set; }
    public string? BucketName { get; private set; }
    public BigString? TemporaryPath { get; private set; }

    protected Archive() { }

    public Archive(BigString awsKey, BigString temporaryPath)
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
