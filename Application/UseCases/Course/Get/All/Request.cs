using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Get.All;

public record Request(
    int? page, 
    int? pageSize
) : IRequest<BaseResponse>;

