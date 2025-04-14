using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SearchRequest = Application.UseCases.Search.Request;

namespace Presentation.Controllers;

[ApiController]
[Route("Search")]
[Authorize]
public class SearchController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int? page = 0, [FromQuery] int? pageSize = 10, CancellationToken cancellationToken = default)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var request = new SearchRequest(query, page, pageSize);
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }
}