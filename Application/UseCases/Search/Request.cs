using Domain.Records;
using MediatR;

namespace Application.UseCases.Search;

/// <summary>
/// Representa a requisição para realizar uma busca combinada de cursos, categorias e professores.
/// </summary>
public record Request : IRequest<BaseResponse>
{
    public string Query { get; }
    public int Page { get; }
    public int PageSize { get; }

    /// <summary>
    /// Inicializa uma nova instância da requisição de busca.
    /// </summary>
    /// <param name="query">Termo de busca.</param>
    /// <param name="page">Número da página (opcional, padrão: 0).</param>
    /// <param name="pageSize">Quantidade de itens por página (opcional, padrão: 10).</param>
    public Request(string query, int? page = null, int? pageSize = null)
    {
        Query = query?.Trim().ToLower() ?? string.Empty;
        Page = page ?? 0;
        PageSize = pageSize ?? 10;
    }
}