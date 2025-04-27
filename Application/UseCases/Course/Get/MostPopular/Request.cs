using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Get.MostPopular;

public record Request : IRequest<BaseResponse<List<Response>>>;