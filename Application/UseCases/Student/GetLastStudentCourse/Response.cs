namespace Application.UseCases.Student.GetLastStudentCourse;

public record Response(
    Guid CourseId,
    string CourseName,
    DateTime EnrollmentDate
);