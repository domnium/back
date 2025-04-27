namespace Application.UseCases.Teacher.GetAll;

public record Response(
    Guid Id,
    string Name,
    string? Tiktok,
    string? Instagram,
    string? GitHub,
    string? Description,
    string? PictureUrl
);