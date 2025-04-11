public record CoursePopularDto(
    Guid Id,
    string? Name,
    string? Description,
    string? ImageUrl,
    decimal? Price,
    long Subscribes,
    List<ModulePopularDto> Modules
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
