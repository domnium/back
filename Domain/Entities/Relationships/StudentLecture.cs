using Domain.Entities.Abstracts;
using Domain.Entities.Core;
using Flunt.Validations;

namespace Domain.Entities.Relationships;

public class StudentLecture : Entity
{
    public Guid StudentId { get; private set; }
    public Guid LectureId { get; private set; }
    public Guid? CourseId { get; private set; }
    public Student Student { get; private set; }
    public Lecture Lecture { get; private set; }
    public Course Course { get; private set; }

    public bool IsCompleted { get; private set; }
    public DateTime? CompletionDate { get; private set; }

    private StudentLecture() {}
    public StudentLecture(Guid studentId, Guid lectureId, Lecture lecture, Student student, Guid? courseId = null)
    {
        StudentId = studentId;
        LectureId = lectureId;
        CourseId = courseId;
        Student = student;
        Lecture = lecture;

        IsCompleted = true;
        CompletionDate = DateTime.UtcNow;

        AddNotifications(new Contract<StudentLecture>()
            .IsNotEmpty(StudentId, "StudentId", "Estudante obrigatório")
            .IsNotEmpty(LectureId, "LectureId", "Aula obrigatória")
        );

        SetValuesCreate();
    }

    public void CompleteLecture()
    {
        IsCompleted = true;
        CompletionDate = DateTime.UtcNow;
        SetValuesUpdate();
    }
}