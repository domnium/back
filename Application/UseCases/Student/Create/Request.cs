using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.Create;

public record Request(
    string? Name,
    Guid UserId,
    bool IsFreeStudent,
    Stream? Picture
) : IRequest<BaseResponse>;
