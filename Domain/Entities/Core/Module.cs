using Domain.Entities.Abstracts;
using Domain.ValueObjects;

namespace Domain.Entities.Core;
public class Module : Entity
{
    public UniqueName Name { get; private set; }
    public Description Description { get; private set; }
    public Course Course { get; private set; }
    public Guid CourseId { get; private set; }

    private readonly IList<Lecture> _lectures = new List<Lecture>();
    public IReadOnlyCollection<Lecture> Lectures => _lectures.ToList();
    
    private Module() {}
    public Module(UniqueName? name, Description? description, Course? course)
    {
        AddNotificationsFromValueObjects(name, description, course);
        Name = name;
        Description = description;
        Course = course;
    }

    public void AddLecture(Lecture lecture)
    {
        if (lecture is null)
        {
            AddNotification("Lecture", "Aula inválida.");
            return;
        }
        _lectures.Add(lecture);
    }
}

