namespace Application.UseCases.Module.GetById;

public record Response(
    Guid Id,
    string Name,
    string Description,
    string CourseName
);