using Domain.Records;
using MediatR;

namespace Application.UseCases.IA.Get.All;

public record Request(
    int skip,
    int take
) : IRequest<BaseResponse>;
