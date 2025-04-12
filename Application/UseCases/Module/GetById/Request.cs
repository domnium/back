using Domain.Records;
using MediatR;

namespace Application.UseCases.Teacher.GetById;

public record Request(
    Guid TeacherId
) : IRequest<BaseResponse>;
