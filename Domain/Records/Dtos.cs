public record CoursePopularDto(
    Guid Id,
    string? Name,
    string? Description,
    string? ImageUrl,
    decimal? Price,
    long Subscribes,
    Guid? StudentId,
    List<ModulePopularDto> Modules,
    string? TrailerUrl = null,
    string? TeacherName = null,
    string? TeacherPictureUrl = null
);

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
