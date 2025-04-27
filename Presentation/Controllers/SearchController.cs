using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SearchRequest = Application.UseCases.Search.Request;
using SearchResponse = Application.UseCases.Search.Response;

using Domain.Records;
using Presentation.Common.Api.Attributes;

namespace Presentation.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações de busca na aplicação.
/// </summary>
[ApiController]
[Route("Search")]
[Authorize]
public class SearchController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Realiza uma busca combinada por cursos, categorias e professores.
    /// </summary>
    /// <param name="query">Termo de busca a ser pesquisado</param>
    /// <param name="page">Número da página para paginação dos resultados (padrão: 0)</param>
    /// <param name="pageSize">Quantidade de itens por página (padrão: 10)</param>
    /// <param name="cancellationToken">Token de cancelamento da operação</param>
    [HttpGet]
    [DefaultResponseTypes(typeof(BaseResponse<SearchResponse>))]
    public async Task<IActionResult> Search(
        [FromQuery] string query,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(new SearchRequest(query, page, pageSize), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}