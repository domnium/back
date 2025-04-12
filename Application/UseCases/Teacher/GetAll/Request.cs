using Domain.Records;
using MediatR;

namespace Application.UseCases.Teacher.GetAll;

public record Request : IRequest<BaseResponse>;
