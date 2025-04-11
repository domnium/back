using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GetStudentRequest = Application.UseCases.Student.GetById.Request;
using GetAllStudentsRequest = Application.UseCases.Student.GetAll.Request;
using CreateStudentRequest = Application.UseCases.Student.Create.Request;
using DeleteStudentRequest = Application.UseCases.Student.Delete.Request;

namespace Presentation.Controllers;

[ApiController]
[Route("Student")]
[Authorize]
public class StudentController(IMediator mediator) : ControllerBase
{
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(new GetStudentRequest( StudentId: id ), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {   
            return StatusCode(500, e.StackTrace);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(new GetAllStudentsRequest(), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response});
        }
        catch(Exception e)
        {   
            return StatusCode(500, e.StackTrace);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateStudentRequest request, CancellationToken cancellationToken)
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

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(DeleteStudentRequest request, CancellationToken cancellationToken)
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
