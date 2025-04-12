using Domain.Records;
using MediatR;

namespace Application.UseCases.Module.GetById;

public record Request(
    Guid ModuleId
) : IRequest<BaseResponse>;
