using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetAll;

public record Request(
    int Skip,
    int Take
) : IRequest<BaseResponse<List<Response>>>;