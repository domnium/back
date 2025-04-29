using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GetModuleRequest = Application.UseCases.Module.GetById.Request;
using GetAllModulesRequest = Application.UseCases.Module.GetAll.Request;
using CreateModuleRequest = Application.UseCases.Module.Create.Request;
using DeleteModuleRequest = Application.UseCases.Module.Delete.Request;

using GetModuleResponse = Application.UseCases.Module.GetById.Response;
using GetAllModulesResponse = Application.UseCases.Module.GetAll.Response;

using Domain.Records;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers;

/// <summary>
/// Controller responsável pelos métodos de retorno, exclusão e criação de Módulos.
/// </summary>
[ApiController]
[Route("Module")]
[Authorize]
public class ModuleController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retorna um módulo pelo seu identificador.
    /// </summary>
    [HttpGet("Get/{id}")]
    [SwaggerOperation(OperationId = "GetModuleById")]
    [ProducesResponseType(typeof(BaseResponse<GetModuleResponse>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetModuleRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna todos os módulos cadastrados.
    /// </summary>
    [HttpGet("Get/All")]
    [SwaggerOperation(OperationId = "GetAllModules")]
    [ProducesResponseType(typeof(BaseResponse<List<GetAllModulesResponse>>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllModulesRequest(), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Cria um novo módulo.
    /// </summary>
    [HttpPost("Create")]
    [SwaggerOperation(OperationId = "CreateModule")]
    [ProducesResponseType(typeof(BaseResponse<object>), 201)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> Create([FromBody] CreateModuleRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Deleta um módulo pelo seu ID.
    /// </summary>
    [HttpDelete("Delete")]
    [SwaggerOperation(OperationId = "DeleteModule")]
    [ProducesResponseType(typeof(BaseResponse<object>), 200)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 404)]
    [ProducesResponseType(typeof(BaseResponse<object>), 409)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> Delete([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteModuleRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}