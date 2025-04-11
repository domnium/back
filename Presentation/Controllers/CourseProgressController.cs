using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CourseProgressRequest = Application.UseCases.CourseProgress.Get.StudentCourseProgress.Request;
using ModuleProgress = Application.UseCases.CourseProgress.Get.StudentModuleProgress.Request;

namespace Presentation.Controllers
{
    [Route("CourseProgress")]
    [ApiController]
    [Authorize]
    public class CourseProgressController(IMediator mediator) : ControllerBase
    {
        [HttpGet("Student/{studentId}/Course/{courseId}")]
        public async Task<IActionResult> GetCourseProgress([FromRoute] Guid courseId, [FromRoute] Guid studentId,
             CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var response =  await mediator.Send(new CourseProgressRequest(studentId, courseId), cancellationToken);
                return StatusCode(response.statuscode, new {response.message, response.Response});
            }
            catch(Exception e)
            {
                return StatusCode(500, e.StackTrace);
            }
        }

        [HttpGet("Student/{studentId}/Module/{moduleId}")]
        public async Task<IActionResult> GetModuleProgress([FromRoute] Guid moduleId, [FromRoute] Guid studentId,
             CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var response = await mediator.Send(new ModuleProgress(studentId, moduleId), cancellationToken);
                return StatusCode(response.statuscode, new {response.message, response.Response});
            }
            catch(Exception e)
            {
                return StatusCode(500, e.StackTrace);
            }
        }
    }
}
