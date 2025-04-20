using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Abstracts;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities.Core;
public class Video : Archive
{
    public bool Ativo { get; private set; }

    [NotMapped]
    public VideoFile? File { get; private set; }

    private Video() {}
    public Video(BigString? temporaryPath, bool ativo = true, VideoFile file = null, 
        ContentType? contentType = null)
    {
        AddNotificationsFromValueObjects(file, temporaryPath);
        File = file;
        Ativo = ativo;
        BucketName = Configuration.BucketVideos;
        ContentType = contentType;
        TemporaryPath = temporaryPath;
    }

    public void Activate() => Ativo = true;
    public void Deactivate() => Ativo = false;
}

