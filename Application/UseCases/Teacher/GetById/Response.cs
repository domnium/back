namespace Application.UseCases.Teacher.GetById;

public record Response(
    Guid Id,
    string Name,
    string? Tiktok,
    string? Instagram,
    string? GitHub,
    string? Description,
    string? PictureUrl
);