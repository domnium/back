using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using GetAllCategoryRequest = Application.UseCases.Category.GetAll.Request;
using CreateCategoryRequest = Application.UseCases.Category.Create.Request;

namespace Presentation.Controllers;

[ApiController]
[Route("Category")]
[Authorize]
public class CategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(new GetAllCategoryRequest(), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {   
            return StatusCode(500, e.StackTrace);
        }
    } 

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateCategoryRequest request ,CancellationToken cancellationToken)
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
}
