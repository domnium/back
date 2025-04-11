using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Get.ById;

public record Request(Guid? id) : IRequest<BaseResponse>;

