using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CreateRequest = Application.UseCases.IA.Create.Request;
using GetAllRequest = Application.UseCases.IA.Get.All.Request;


namespace Presentation.Controllers;

[ApiController]
[Route("IA")]
[Authorize]
public class IAController(IMediator mediator) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] CreateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response, response.notifications}); 
        }
        catch(Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

     [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll(int skip, int take, CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(new GetAllRequest(skip, take), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response}); 
        }
        catch(Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
