using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetAll;

public record Request : IRequest<BaseResponse>;
