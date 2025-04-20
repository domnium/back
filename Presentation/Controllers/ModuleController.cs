using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GetModuleRequest = Application.UseCases.Module.GetById.Request;
using GetAllModulesRequest = Application.UseCases.Module.GetAll.Request;
using CreateModuleRequest = Application.UseCases.Module.Create.Request;
using DeleteModuleRequest = Application.UseCases.Module.Delete.Request;

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
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetModuleRequest(id), cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }

    /// <summary>
    /// Retorna todos os módulos cadastrados.
    /// </summary>
    [HttpGet("Get/All")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllModulesRequest(), cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }

    /// <summary>
    /// Cria um novo módulo.
    /// </summary>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateModuleRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }

    /// <summary>
    /// Deleta um módulo pelo seu ID.
    /// </summary>
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteModuleRequest(id), cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }
}
