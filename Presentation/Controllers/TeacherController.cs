using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GetTeacherRequest = Application.UseCases.Teacher.GetById.Request;
using GetAllTeachersRequest = Application.UseCases.Teacher.GetAll.Request;
using CreateTeacherRequest = Application.UseCases.Teacher.Create.Request;
using DeleteTeacherRequest = Application.UseCases.Teacher.Delete.Request;

namespace Presentation.Controllers;

/// <summary>
/// Controller responsável pelos métodos de retorno, exclusão e criação de Professores.
/// </summary>
[ApiController]
[Route("Teacher")]
[Authorize]
public class TeacherController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Método responsável por retornar um professor do sistema pelo seu identificador.
    /// </summary>
    /// <param name="Id">Identificador para o filtro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="IActionResult"/> com status e objeto encontrado</returns>
    [HttpGet("Get/{Id}")]
    public async Task<IActionResult> GetById([FromQuery] Guid Id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetTeacherRequest(TeacherId: Id), cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }

    /// <summary>
    /// Método responsável por retornar até 100 professores do sistema.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="IActionResult"/> com status e objeto encontrado</returns>
    [HttpGet("Get/All")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllTeachersRequest(), cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }

    /// <summary>
    /// Método responsável por criar um professor no sistema.
    /// </summary>
    /// <param name="request">Objeto com os parâmetros necessários para a criação de um professor</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="IActionResult"/> com status e objeto encontrado</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] CreateTeacherRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response, response.notifications });
    }

    /// <summary>
    /// Método responsável por deletar um professor no sistema.
    /// </summary>
    /// <param name="Id">Identificador para a exclusão</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="IActionResult"/> com status e objeto encontrado</returns>
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid Id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteTeacherRequest(TeacherId: Id), cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }
}
