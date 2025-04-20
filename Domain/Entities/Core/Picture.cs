using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Abstracts;
using Domain.Entities.Core;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Picture : Archive
{
    public bool Ativo { get; private set; }
    
    [NotMapped]
    public AppFile? File { get; private set;}

    private Picture() {}
    public Picture(BigString? awsKey, bool ativo = true, AppFile appFile = null,
     BigString? temporaryPath = null, ContentType? contentType = null)
    {
        AddNotificationsFromValueObjects(appFile);
        File = appFile;
        Ativo = ativo;
        AwsKey = awsKey;
        TemporaryPath = temporaryPath;
        BucketName = Configuration.BucketArchives;
        ContentType = contentType;
    }

    public void Activate() => Ativo = true;
    public void Deactivate() => Ativo = false;
}
