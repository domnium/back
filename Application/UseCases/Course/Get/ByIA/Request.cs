using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Get.ByIA;

public record Request(
    Guid IAId, int? Page, int? PageSize
) : IRequest<BaseResponse>;
