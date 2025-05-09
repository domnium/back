public record CoursePopularDto(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl,
    decimal Price,
    long Subscribes,
    Guid? StudentId,
    List<ModulePopularDto> Modules,
    string? TrailerUrl = null,
    string? TeacherName = null,
    string? TeacherPictureUrl = null
)
{
    public CoursePopularDto(
        Guid id,
        string name,
        string description,
        string imageUrl,
        decimal price,
        long subscribes,
        Guid studentId,
        List<ModulePopularDto> modules
    ) : this(id, name, description, imageUrl, price, subscribes, studentId, modules, null, null, null)
    {
    }
}

public record ModulePopularDto(
    Guid Id,
    string? Name,
    List<LecturePopularDto> Lectures
);

public record LecturePopularDto(
    Guid Id,
    string? Name,
    string? Duration
);
