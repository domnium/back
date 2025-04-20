using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CreateRequest = Application.UseCases.IA.Create.Request;
using GetAllRequest = Application.UseCases.IA.Get.All.Request;

namespace Presentation.Controllers;

/// <summary>
/// Controlador responsável pelas operações com entidades de Inteligência Artificial (IA).
/// </summary>
[ApiController]
[Route("IA")]
[Authorize]
public class IAController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Cria uma nova IA.
    /// </summary>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] CreateRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response, response.notifications });
    }

    /// <summary>
    /// Retorna todas as IAs cadastradas com paginação.
    /// </summary>
    [HttpGet("Get/All")]
    public async Task<IActionResult> GetAll([FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllRequest(skip, take), cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response });
    }
}
