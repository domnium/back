using Domain.Records;
using MediatR;

namespace Application.UseCases.Teacher.Delete;

public record Request(
    Guid TeacherId
) : IRequest<BaseResponse<object>>;
