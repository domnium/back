namespace Application.UseCases.Student.GetAll;

public record Response(
    Guid Id,
    string Name,
    string? PictureUrl
);