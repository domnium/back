using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Delete;
public record Request(Guid? id) : IRequest<BaseResponse>;
