using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Get.ByCategory;

public record Request(
    Guid CategoryId, 
    int? page, 
    int? pageSize
) : IRequest<BaseResponse>;
