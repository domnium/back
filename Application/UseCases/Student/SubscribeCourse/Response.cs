namespace Application.UseCases.Student.SubscribeCourse;

public record Response(
    Guid StudentId,
    Guid CourseId,
    string StudentName,
    string CourseName,
    DateTime EnrollmentDate
);