using Domain.Entities.Abstracts;
using Domain.ValueObjects;

namespace Domain.Entities.Core;
public sealed class Parameter : Entity
{
    public UniqueName? Name { get; private set;}
    public Description? Description { get;private set;}
    public bool? FreeCourse { get; private set;}
    public Course? Course { get; private set;}
    public Guid? CourseId {get; private set;}

    private Parameter() {}
    public Parameter(UniqueName? name, Description? description, bool? freeCourse, Course? course)
    {
        AddNotificationsFromValueObjects(name, description, course);
        Name = name;
        Description = description;
        FreeCourse = freeCourse;
        Course = course;
        CourseId = course.Id;
    }
}

