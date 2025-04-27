namespace Application.UseCases.Module.GetAll;

public record Response(
    Guid Id,
    string Name,
    string Description,
    string CourseName
);