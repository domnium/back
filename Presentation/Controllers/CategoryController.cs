using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GetAllCategoryRequest = Application.UseCases.Category.GetAll.Request;
using GetByIdRequest = Application.UseCases.Category.GetById.Request;
using CreateCategoryRequest = Application.UseCases.Category.Create.Request;

using GetAllCategoryResponse = Application.UseCases.Category.GetAll.Response;
using GetByIdCategoryResponse = Application.UseCases.Category.GetById.Response;

using DeleteCategoryRequest = Application.UseCases.Category.Delete.Request;
using Domain.Records;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

/// <summary>
/// Controlador responsável pelas operações de categorias (Category),
/// incluindo criação, consulta, listagem e remoção.
/// </summary>
[Route("Category")]
[Authorize]
public class CategoryController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retorna todas as categorias cadastradas.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de categorias</returns>
    [HttpGet("Get/All")]
    [SwaggerOperation(OperationId = "GetAllCategories")]
    [ProducesResponseType(typeof(BaseResponse<List<GetAllCategoryResponse>>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> GetAll([FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllCategoryRequest(skip, take), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna os dados de uma categoria específica pelo seu ID.
    /// </summary>
    /// <param name="Id">ID da categoria</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Categoria encontrada</returns>
    [HttpGet("Get/ById")]
    [SwaggerOperation(OperationId = "GetCategoryById")]
    [ProducesResponseType(typeof(BaseResponse<GetByIdCategoryResponse>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> GetById([FromQuery] Guid Id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetByIdRequest(Id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Cria uma nova categoria com nome, descrição e imagem.
    /// </summary>
    /// <param name="request">Objeto contendo os dados da categoria</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status de criação e dados da categoria</returns>
    [HttpPost("Create")]
    [SwaggerOperation(OperationId = "CreateCategory")]
    [ProducesResponseType(typeof(BaseResponse<GetByIdCategoryResponse>), 201)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> Create([FromForm] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Exclui uma categoria com base no seu ID.
    /// </summary>
    /// <param name="Id">ID da categoria a ser removida</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da operação</returns>
    [HttpDelete("Delete")]
    [SwaggerOperation(OperationId = "DeleteCategory")]
    [ProducesResponseType(typeof(BaseResponse<object>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> Delete([FromQuery] Guid Id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteCategoryRequest(Id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}