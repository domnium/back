using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetById;

public record Request(
    Guid StudentId
) : IRequest<BaseResponse>;
