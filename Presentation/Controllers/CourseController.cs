using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CreateRequest = Application.UseCases.Course.Create.Request;
using DeleteRequest = Application.UseCases.Course.Delete.Request;
using GetAllRequest = Application.UseCases.Course.Get.All.Request;
using GetByCategory = Application.UseCases.Course.Get.ByCategory.Request;
using GetByIA = Application.UseCases.Course.Get.ByIA.Request;
using GetMostPopular = Application.UseCases.Course.Get.MostPopular.Request;
using GetByIdRequest = Application.UseCases.Course.Get.ById.Request;

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

    [HttpGet("Get/All")]
    public async Task<IActionResult> GetAll(int page, int pageSize, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetAllRequest(page, pageSize), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    [HttpGet("Get/ByCategory/{categoryId}/Page/{page}/PageSize/{pageSize}")]
    public async Task<IActionResult> GetByCategory(Guid categoryId, int page,
         int pageSize, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetByCategory(categoryId, 
                page, pageSize), cancellationToken);

            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    [HttpGet("Get/ById/{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetByIdRequest(id), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    [HttpGet("Get/ByIA/{IAId}/Page/{page}/PageSize/{pageSize}")]
    public async Task<IActionResult> GetByIA(Guid IAId, int page, int pageSize, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetByIA(IAId, page, pageSize), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    [HttpGet("Get/MostPopular")]
    public async Task<IActionResult> GetMostPopular(CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetMostPopular(), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new DeleteRequest(id), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }
}
