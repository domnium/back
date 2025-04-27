using Domain.Records;
using MediatR;

namespace Application.UseCases.Teacher.GetAll;

/// <summary>
/// Representa a requisição para retornar até 100 professores do sistema.
/// </summary>
public record Request : IRequest<BaseResponse<List<Response>>>;