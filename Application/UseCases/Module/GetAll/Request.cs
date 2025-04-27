using Domain.Records;
using MediatR;

namespace Application.UseCases.Module.GetAll;

public record Request() : IRequest<BaseResponse<List<Response>>>;