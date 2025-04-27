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
using Presentation.Common.Api.Attributes;

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
    [DefaultResponseTypes(typeof(BaseResponse<GetModuleResponse>))]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetModuleRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna todos os módulos cadastrados.
    /// </summary>
    [HttpGet("Get/All")]
    [DefaultResponseTypes(typeof(BaseResponse<List<GetAllModulesResponse>>))]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllModulesRequest(), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Cria um novo módulo.
    /// </summary>
    [HttpPost("Create")]
    [DefaultResponseTypes(typeof(BaseResponse<object>))]
    public async Task<IActionResult> Create([FromBody] CreateModuleRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Deleta um módulo pelo seu ID.
    /// </summary>
    [HttpDelete("Delete")]
    [DefaultResponseTypes(typeof(BaseResponse<object>))]
    public async Task<IActionResult> Delete([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteModuleRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}