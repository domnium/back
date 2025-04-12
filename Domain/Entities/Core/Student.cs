using Domain.Entities.Abstracts;
using Domain.Entities.Relationships;
using Domain.ValueObjects;

namespace Domain.Entities.Core;

public class Student : Entity
{
    public UniqueName Name { get; private set; }
    public Picture Picture { get; private set; }
    public Guid PictureId { get; private set; }
    public User User { get; private set; }
    public Guid  UserId { get; private set; }
    public bool IsFreeStudent { get; private set; }
    private readonly IList<StudentLecture> _studentLectures = new List<StudentLecture>();
    public IReadOnlyCollection<StudentLecture> StudentLectures => _studentLectures.ToList();
    private readonly IList<StudentCourse> _studentCourses = new List<StudentCourse>();
    public IReadOnlyCollection<StudentCourse> StudentCourses => _studentCourses.ToList();

    private readonly IList<Subscription> _subscriptions = new List<Subscription>();
    public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.ToList();

    private Student(){}

    public Student(UniqueName name, User user, bool isFreeStudent, Picture picture)
    {
        Name = name;
        User = user;
        IsFreeStudent = isFreeStudent;
        Picture = picture;

        AddNotificationsFromValueObjects(name, user, picture);
        SetValuesCreate();
    }

    public void AddSubscription(Subscription subscription)
    {
        if (subscription == null)
        {
            AddNotification("Subscription", "Assinatura inválida.");
            return;
        }

        AddNotifications(subscription);
        _subscriptions.Add(subscription);
    }

    public bool HasSubscriptionToCourse(Guid courseId)
    {
        return _studentCourses.Any(sc => sc.CourseId == courseId);
    }

    public void EnrollInCourse(Course course)
    {
        if (course is null)
        {
            AddNotification("Course", "Curso inválido.");
            return;
        }

        if (HasSubscriptionToCourse(course.Id))
        {
            AddNotification("Course", "Aluno já está inscrito neste curso.");
            return;
        }
        var studentCourse = new StudentCourse(this.Id, course.Id, this, course);
        _studentCourses.Add(studentCourse);
    }

    public void CompleteLecture(Lecture lecture)
    {
        if (lecture is null)
        {
            AddNotification("Lecture", "Aula inválida.");
            return;
        }

        var existing = _studentLectures.FirstOrDefault(sl => sl.LectureId == lecture.Id);
        if (existing is not null)
        {
            existing.CompleteLecture(); 
            return;
        }

        var newLecture = new StudentLecture(
            lecture: lecture,
            student: this,
            courseId: lecture.Module?.Course?.Id
        );
        _studentLectures.Add(newLecture);
    }
}