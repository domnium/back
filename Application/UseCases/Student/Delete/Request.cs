using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.Delete;

public record Request(
    Guid StudentId
) : IRequest<BaseResponse<object>>;
