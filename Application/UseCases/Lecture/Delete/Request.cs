using Domain.Records;
using MediatR;

namespace Application.UseCases.Lecture.Delete;

public record Request(
    Guid id
) : IRequest<BaseResponse<object>>;
