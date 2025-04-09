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

    protected Archive() { }

    public Archive(BigString awsKey)
    {
        AwsKey = awsKey;
        AddNotifications(awsKey);
    }

    public void SetTemporaryUrl(Url url, DateTime expiration)
    {
        UrlTemp = url;
        UrlExpired = expiration;
    }

    public void SetBucket(string bucket)
    {
        BucketName = bucket;
    }
    
    public Guid? SetGuid(Guid id)
    {
        if (AwsKey is null) return null;
        Id = Guid.NewGuid();
        return Id;
    }
}
