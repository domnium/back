using Flunt.Notifications;

namespace Application.UseCases.Category.GetAll;

public record Response(
    Guid? CategoryId,
    string? Name,
    string? Description,
    string? ImageUrl
);