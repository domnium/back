using System;
using Domain.Entities.Abstracts;
using Domain.ValueObjects;

namespace Domain.Entities.Core;

public class IA : Entity
{
    public UniqueName? Name { get; private set; }
    public Picture? Picture { get; private set; }
    public Guid PictureId { get; private set; }
    public List<Course> Courses { get; private set; }

    private IA() {}
    public IA(UniqueName name, Picture picture)
    {
        AddNotificationsFromValueObjects(name, picture);
        Name = name;
        Picture = picture;
        PictureId = picture.Id;
        Picture.SetPictureOwner(this);
    }
}
