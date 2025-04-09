
using Domain.Entities.Abstracts;
using Domain.ValueObjects;

namespace Domain.Entities.Core;
public class Category : Entity
{   
    public UniqueName Name { get; private set; }
    public Description Description { get; private set; }
    public Picture Image { get; private set; }
    public Guid PictureId {get; private set;}
    private readonly IList<Course> _courses = new List<Course>();
    public IReadOnlyCollection<Course> Courses => _courses.ToList();

    private Category() {}
    public Category(UniqueName name, Description description, Picture? image = null)
    {
        AddNotificationsFromValueObjects(name, description);
        Name = name;
        Description = description;
        Image = image;
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

