using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CreateRequest = Application.UseCases.Course.Create.Request;
using DeleteRequest = Application.UseCases.Course.Delete.Request;
using GetAllRequest = Application.UseCases.Course.Get.All.Request;
using GetByCategory = Application.UseCases.Course.Get.ByCategory.Request;
using GetByIA = Application.UseCases.Course.Get.ByIA.Request;
using GetMostPopular = Application.UseCases.Course.Get.MostPopular.Request;
using GetByIdRequest = Application.UseCases.Course.Get.ById.Request;

using GetAllResponse = Application.UseCases.Course.Get.All.Response;
using GetByCategoryResponse = Application.UseCases.Course.Get.ByCategory.Response;
using GetByIAResponse = Application.UseCases.Course.Get.ByIA.Response;
using GetMostPopularResponse = Application.UseCases.Course.Get.MostPopular.Response;
using GetByIdResponse = Application.UseCases.Course.Get.ById.Response;

using Domain.Records;
using Presentation.Common.Api.Attributes;

namespace Presentation.Controllers;

/// <summary>
/// Controlador responsável por operações relacionadas a cursos (Course),
/// incluindo criação, consulta por categoria, IA, popularidade e exclusão.
/// </summary>
[ApiController]
[Route("Course")]
[Authorize]
public class CourseController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Cria um novo curso.
    /// </summary>
    [HttpPost("Create")]
    [DefaultResponseTypes(typeof(BaseResponse<object>))]
    public async Task<IActionResult> Create([FromForm] CreateRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna todos os cursos paginados.
    /// </summary>
    [HttpGet("Get/All")]
    [DefaultResponseTypes(typeof(BaseResponse<List<GetAllResponse>>))]
    public async Task<IActionResult> GetAll([FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllRequest(skip, take), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna cursos filtrados por categoria com paginação.
    /// </summary>
    [HttpGet("Get/ByCategory")]
    [DefaultResponseTypes(typeof(BaseResponse<List<GetByCategoryResponse>>))]
    public async Task<IActionResult> GetByCategory([FromQuery] Guid categoryId, [FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetByCategory(categoryId, skip, take), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna detalhes de um curso pelo ID.
    /// </summary>
    [HttpGet("Get/ById/{id}")]
    [DefaultResponseTypes(typeof(BaseResponse<GetByIdResponse>))]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetByIdRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna cursos associados a uma IA específica, com paginação.
    /// </summary>
    [HttpGet("Get/ByIA")]
    [DefaultResponseTypes(typeof(BaseResponse<List<GetByIAResponse>>))]
    public async Task<IActionResult> GetByIA([FromQuery] Guid IAId, [FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetByIA(IAId, skip, take), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna os cursos mais populares.
    /// </summary>
    [HttpGet("Get/MostPopular")]
    [DefaultResponseTypes(typeof(BaseResponse<List<GetMostPopularResponse>>))]
    public async Task<IActionResult> GetMostPopular(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetMostPopular(), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Exclui um curso pelo seu ID.
    /// </summary>
    [HttpDelete("Delete/{id}")]
    [DefaultResponseTypes(typeof(BaseResponse<object>))]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}