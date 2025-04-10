using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GetAllCategoryRequest = Application.UseCases.Category.GetAll.Request;
using GetByIdRequest = Application.UseCases.Category.GetById.Request;
using CreateCategoryRequest = Application.UseCases.Category.Create.Request;
using DeleteCategoryRequest = Application.UseCases.Category.Delete.Request;

namespace Presentation.Controllers;

[ApiController]
[Route("Category")]
[Authorize]
public class CategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
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

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] Guid Id, CancellationToken cancellationToken )
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetByIdRequest(Id), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {   
            return StatusCode(500, e.StackTrace);
        }
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken cancellationToken)
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


    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid Id, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new DeleteCategoryRequest(Id), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {   
            return StatusCode(500, e.StackTrace);
        }
    } 
}
