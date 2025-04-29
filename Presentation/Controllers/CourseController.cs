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
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(OperationId = "CreateCourse")]
    [ProducesResponseType(typeof(BaseResponse<object>), 201)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> Create([FromForm] CreateRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna todos os cursos paginados.
    /// </summary>
    [HttpGet("Get/All")]
    [SwaggerOperation(OperationId = "GetAllCourses")]
    [ProducesResponseType(typeof(BaseResponse<List<GetAllResponse>>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> GetAll([FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllRequest(skip, take), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna cursos filtrados por categoria com paginação.
    /// </summary>
    [HttpGet("Get/ByCategory")]
    [SwaggerOperation(OperationId = "GetCoursesByCategory")]
    [ProducesResponseType(typeof(BaseResponse<List<GetByCategoryResponse>>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> GetByCategory([FromQuery] Guid categoryId, [FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetByCategory(categoryId, skip, take), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna detalhes de um curso pelo ID.
    /// </summary>
    [HttpGet("Get/ById/{id}")]
    [SwaggerOperation(OperationId = "GetCourseById")]
    [ProducesResponseType(typeof(BaseResponse<GetByIdResponse>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetByIdRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna cursos associados a uma IA específica, com paginação.
    /// </summary>
    [HttpGet("Get/ByIA")]
    [SwaggerOperation(OperationId = "GetCoursesByIA")]
    [ProducesResponseType(typeof(BaseResponse<List<GetByIAResponse>>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> GetByIA([FromQuery] Guid IAId, [FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetByIA(IAId, skip, take), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna os cursos mais populares.
    /// </summary>
    [HttpGet("Get/MostPopular")]
    [SwaggerOperation(OperationId = "GetMostPopularCourses")]
    [ProducesResponseType(typeof(BaseResponse<List<GetMostPopularResponse>>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> GetMostPopular(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetMostPopular(), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Exclui um curso pelo seu ID.
    /// </summary>
    [HttpDelete("Delete")]
    [SwaggerOperation(OperationId = "DeleteCourse")]
    [ProducesResponseType(typeof(BaseResponse<object>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> Delete([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}