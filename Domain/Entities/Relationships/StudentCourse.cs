using Domain.Entities.Abstracts;
using Domain.Entities.Core;
using Flunt.Validations;

namespace Domain.Entities.Relationships;
public class StudentCourse : Entity
{
    public Guid StudentId { get; private set; }
    public Guid CourseId { get; private set; }
    public Student Student { get; private set; }
    public Course Course { get; private set; }
    public DateTime EnrollmentDate { get; private set; }

    private StudentCourse() {}
    public StudentCourse(Guid studentId, Guid courseId, Student student, Course course)
    {
        StudentId = studentId;
        CourseId = courseId;
        Student = student;
        Course = course;
        EnrollmentDate = DateTime.UtcNow;

        AddNotifications(new Contract<StudentCourse>()
            .IsNotEmpty(StudentId, "StudentId", "Estudante obrigatório")
            .IsNotEmpty(CourseId, "CourseId", "Curso obrigatório")
        );
    }
}
