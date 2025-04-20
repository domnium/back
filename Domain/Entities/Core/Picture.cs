using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Abstracts;
using Domain.Entities.Core;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Picture : Archive
{
    [NotMapped]
    public AppFile? File { get; private set;}
    public Teacher? Teacher {get; private set;}
    public Guid? TeacherId {get; private set;}
    public Student? Student  {get; private set;}
    public Guid? StudentId  {get; private set;}
    public IA? IA  {get; private set;}
    public Guid? IAId  {get; private set;}
    public Guid? CourseId { get; private set; }
    public Course? Course { get; private set; }
    public Category? Category {get; private set;}
    public Guid? CategoryId {get; private set;}

    private Picture() {}
    public Picture(BigString? awsKey, bool ativo = false, AppFile appFile = null,
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

    public void SetPictureOwner(object owner)
    {
        switch (owner)
        {
            case Teacher t:
                Teacher = t;
                TeacherId = t.Id;
                break;

            case Student s:
                Student = s;
                StudentId = s.Id;
                break;

            case IA ia:
                IA = ia;
                IAId = ia.Id;
                break;

            case Course c:
                Course = c;
                CourseId = c.Id;
                break;

            case Category cat:
                Category = cat;
                CategoryId = cat.Id;
                break;

            default:
                throw new ArgumentException("Tipo de entidade não suportado para Picture.");
        }
    }
}
