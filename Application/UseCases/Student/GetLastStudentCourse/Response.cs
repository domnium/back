namespace Application.UseCases.Student.GetLastStudentCourse;

public record Response(
    Guid? CourseId,
    string? CourseName,
    string? Description,
    string? ImageUrl,
    Guid? StudentId
);