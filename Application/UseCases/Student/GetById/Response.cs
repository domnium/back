namespace Application.UseCases.Student.GetById;

public record Response(
    Guid Id,
    string Name,
    string? PictureUrl
);