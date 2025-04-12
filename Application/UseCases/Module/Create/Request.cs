using Domain.Records;
using MediatR;

namespace Application.UseCases.Module.Create;

public record Request(
    string? Name,
    string? Description,
    Guid CourseId
) : IRequest<BaseResponse>;
