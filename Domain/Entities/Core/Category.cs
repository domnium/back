
using Domain.Entities.Abstracts;
using Domain.ValueObjects;

namespace Domain.Entities.Core;
public class Category : Entity
{   
    public UniqueName Name { get; private set; }
    public Description Description { get; private set; }
    public Picture Picture { get; private set; }
    public Guid PictureId {get; private set;}
    private readonly IList<Course> _courses = new List<Course>();
    public IReadOnlyCollection<Course> Courses => _courses.ToList();

    private Category() {}
    public Category(UniqueName name, Description description, Picture? image)
    {
        AddNotificationsFromValueObjects(name, description, image);
        Name = name;
        Description = description;
        Picture = image;
        PictureId = Picture.Id;
    }

    public void AddCourse(Course course)
    {
        if (course is null)
        {
            AddNotification("Course", "Curso inválido.");
            return;
        }
        _courses.Add(course);
    }
}

