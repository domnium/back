using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Abstracts;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities.Core;
public class Video : Archive
{
    [NotMapped]
    public VideoFile? File { get; private set; }
    public Lecture? Lecture { get; private set; }
    public Guid? LectureId {get; private set;}
    public Guid? CourseId { get; private set; }
    public Course? Course {get; private set;}

    private Video() {}
    public Video(BigString? temporaryPath, bool ativo = false, VideoFile file = null, 
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

    public void SetVideoOwner(object owner)
    {
        switch (owner)
        {
            case Lecture l:
                Lecture = l;
                LectureId = l.Id;
                break;

            case Course c:
                Course = c;
                CourseId = c.Id;
                break;

            default:
                throw new ArgumentException("Tipo de entidade não suportado para Video.");
        }
    }

}

