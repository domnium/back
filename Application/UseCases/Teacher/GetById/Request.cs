using Domain.Records;
using MediatR;

namespace Application.UseCases.Teacher.GetById;

/// <summary>
/// Representa a requisição para retornar um professor pelo identificador.
/// </summary>
public record Request(Guid TeacherId) : IRequest<BaseResponse<Response>>;