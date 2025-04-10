using System;
using Domain.Entities.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CreateRequest = Application.UseCases.Course.Create.Request;

namespace Presentation.Controllers;

[ApiController]
[Route("Course")]
[Authorize]
public class CourseController(IMediator mediator) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateRequest request ,CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
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
}
