using System;
using Domain.Entities.Abstracts;
using Domain.ValueObjects;

namespace Domain.Entities.Core;

public class IA : Entity
{
    public UniqueName? Name { get; private set; }
    public Picture? Picture { get; private set; }
    public List<Course> Courses { get; private set; }
    public IA(UniqueName name, Picture picture)
    {
        AddNotificationsFromValueObjects(name, picture);
        Name = name;
        Picture = picture;
        SetValuesCreate();
    }
}
