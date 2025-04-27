using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CreateRequest = Application.UseCases.IA.Create.Request;
using DeleteRequest = Application.UseCases.IA.Delete.Request;
using GetAllRequest = Application.UseCases.IA.Get.All.Request;

using GetAllResponse = Application.UseCases.IA.Get.All.Response;

using Domain.Records;
using Presentation.Common.Api.Attributes;

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
    [DefaultResponseTypes(typeof(BaseResponse<object>))]
    public async Task<IActionResult> Create([FromForm] CreateRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna todas as IAs cadastradas com paginação.
    /// </summary>
    [HttpGet("Get/All")]
    [DefaultResponseTypes(typeof(BaseResponse<List<GetAllResponse>>))]
    public async Task<IActionResult> GetAll([FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllRequest(skip, take), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Deleta registro de IA no banco.
    /// </summary>
    [HttpDelete("Delete")]
    [DefaultResponseTypes(typeof(BaseResponse<object>))]
    public async Task<IActionResult> Delete([FromQuery] Guid Id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteRequest(Id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}