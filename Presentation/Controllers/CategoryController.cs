using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GetAllCategoryRequest = Application.UseCases.Category.GetAll.Request;
using GetByIdRequest = Application.UseCases.Category.GetById.Request;
using CreateCategoryRequest = Application.UseCases.Category.Create.Request;
using DeleteCategoryRequest = Application.UseCases.Category.Delete.Request;

namespace Presentation.Controllers;

/// <summary>
/// Controlador responsável pelas operações de categorias (Category),
/// incluindo criação, consulta, listagem e remoção.
/// </summary>
[ApiController]
[Route("Category")]
[Authorize]
public class CategoryController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retorna todas as categorias cadastradas.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de categorias ou erro interno</returns>
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetAllCategoryRequest(), cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.Response });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Retorna os dados de uma categoria específica pelo seu ID.
    /// </summary>
    /// <param name="Id">ID da categoria</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Categoria encontrada ou mensagem de erro</returns>
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] Guid Id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetByIdRequest(Id), cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.Response });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Cria uma nova categoria com nome, descrição e imagem.
    /// </summary>
    /// <param name="request">Objeto contendo os dados da categoria</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status de criação e dados da categoria</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.Response });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Exclui uma categoria com base no seu ID.
    /// </summary>
    /// <param name="Id">ID da categoria a ser removida</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da operação</returns>
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid Id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new DeleteCategoryRequest(Id), cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.Response });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }
}
