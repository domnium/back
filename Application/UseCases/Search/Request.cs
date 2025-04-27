using Domain.Records;
using MediatR;

namespace Application.UseCases.Search;

/// <summary>
/// Representa a requisição para realizar uma busca combinada de cursos, categorias e professores.
/// </summary>
public record Request(
    string Query,
    int Page = 0,
    int PageSize = 10
) : IRequest<BaseResponse<List<Response>>>;