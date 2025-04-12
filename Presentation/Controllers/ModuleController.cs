using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GetModuleRequest = Application.UseCases.Module.GetById.Request;
using GetAllModulesRequest = Application.UseCases.Module.GetAll.Request;
using CreateModuleRequest = Application.UseCases.Module.Create.Request;
using DeleteModuleRequest = Application.UseCases.Module.Delete.Request;

namespace Presentation.Controllers;

/// <summary>
/// Controller responsável pelos métodos de retorno, exclução e criação de Módulos.
/// </summary>
[ApiController]
[Route("Module")]
[Authorize]
public class ModuleController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Método responsável por retornar um módulo do sistema pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador para o filtro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="IActionResult"/> com status e objeto encontrado</returns>
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(new GetModuleRequest( ModuleId: id ), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {   
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Método responsável por retornar até 100 módulos do sistema.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="IActionResult"/> com status e objeto encontrado</returns>
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(new GetAllModulesRequest(), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {   
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Método responsável por criar um módulo no sistema.
    /// </summary>
    /// <param name="request">Objeto com os parâmetros necessários para a criação de um módulo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="IActionResult"/> com status e objeto encontrado</returns>
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateModuleRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {   
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Método responsável por deletar um módulo no sistema.
    /// </summary>
    /// <param name="id">Identificador para a exclução</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="IActionResult"/> com status e objeto encontrado</returns>
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(new DeleteModuleRequest( ModuleId: id ), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {   
            return StatusCode(500, e.StackTrace);
        }
    } 
}
