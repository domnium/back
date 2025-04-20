using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GetStudentRequest = Application.UseCases.Student.GetById.Request;
using GetAllStudentsRequest = Application.UseCases.Student.GetAll.Request;
using CreateStudentRequest = Application.UseCases.Student.Create.Request;
using DeleteStudentRequest = Application.UseCases.Student.Delete.Request;

namespace Presentation.Controllers;

/// <summary>
/// Controller responsável pelos métodos de retorno, exclusão e criação de Estudantes.
/// </summary>
[ApiController]
[Route("Student")]
[Authorize]
public class StudentController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retorna um estudante pelo seu identificador.
    /// </summary>
    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetStudentRequest(id), cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }

    /// <summary>
    /// Retorna todos os estudantes cadastrados.
    /// </summary>
    [HttpGet("Get/All")]
    public async Task<IActionResult> GetAll([FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllStudentsRequest(skip, take), cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }

    /// <summary>
    /// Cria um novo estudante.
    /// </summary>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] CreateStudentRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }

    /// <summary>
    /// Deleta um estudante pelo seu ID.
    /// </summary>
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteStudentRequest(id), cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }
}
