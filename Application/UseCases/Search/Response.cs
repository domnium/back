namespace Application.UseCases.Search;

public record Response(
    string Type,
    Guid Id,
    string Name,
    string? Description,
    string? ImageUrl,
    decimal? Price
);