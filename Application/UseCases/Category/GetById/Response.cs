namespace Application.UseCases.Category.GetById;

public record Response(
    Guid? CategoryId,
    string? Name,
    string? Description,
    string? ImageUrl
);