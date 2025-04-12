using Domain.Records;
using MediatR;

namespace Application.UseCases.Module.Delete;

public record Request(
    Guid ModuleId
) : IRequest<BaseResponse>;
