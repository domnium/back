using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GetStudentRequest = Application.UseCases.Student.GetById.Request;
using GetAllStudentsRequest = Application.UseCases.Student.GetAll.Request;
using CreateStudentRequest = Application.UseCases.Student.Create.Request;
using DeleteStudentRequest = Application.UseCases.Student.Delete.Request;
using CourseProgressRequest = Application.UseCases.Student.GetStudentCourseProgress.Request;
using ModuleProgress = Application.UseCases.Student.GetStudentModuleProgress.Request;
using GetLastStudentCourse = Application.UseCases.Student.GetLastStudentCourse.Request;
using Subscribe = Application.UseCases.Student.SubscribeCourse.Request;

using GetStudentResponse = Application.UseCases.Student.GetById.Response;
using GetAllStudentsResponse = Application.UseCases.Student.GetAll.Response;
using CourseProgressResponse = Application.UseCases.Student.GetStudentCourseProgress.Response;
using ModuleProgressResponse = Application.UseCases.Student.GetStudentModuleProgress.Response;
using GetLastStudentCourseResponse = Application.UseCases.Student.GetLastStudentCourse.Response;
using SubscribeResponse = Application.UseCases.Student.SubscribeCourse.Response;

using Domain.Records;
using Presentation.Common.Api.Attributes;

namespace Presentation.Controllers;

/// <summary>
/// Controller responsável pelos métodos de retorno, exclusão e criação de Estudantes.
/// </summary>
[ApiController]
[Route("Student")]
[Authorize]
public class StudentController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retorna um estudante pelo seu identificador.
    /// </summary>
    [HttpGet("Get/{id}")]
    [DefaultResponseTypes(typeof(BaseResponse<GetStudentResponse>))]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetStudentRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna todos os estudantes cadastrados.
    /// </summary>
    [HttpGet("Get/All")]
    [DefaultResponseTypes(typeof(BaseResponse<List<GetAllStudentsResponse>>))]
    public async Task<IActionResult> GetAll([FromQuery] int skip, [FromQuery] int take, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllStudentsRequest(skip, take), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna o progresso do estudante em um curso específico.
    /// </summary>
    [HttpGet("CourseProgress")]
    [DefaultResponseTypes(typeof(BaseResponse<CourseProgressResponse>))]
    public async Task<IActionResult> GetCourseProgress(
        [FromQuery] Guid courseId,
        [FromQuery] Guid studentId,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CourseProgressRequest(studentId, courseId), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna o último curso acessado pelo estudante.
    /// </summary>
    [HttpGet("GetLastStudentCourse")]
    [DefaultResponseTypes(typeof(BaseResponse<GetLastStudentCourseResponse>))]
    public async Task<IActionResult> GetLastStudentCourse(
        [FromQuery] Guid studentId,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetLastStudentCourse(studentId), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna o progresso do estudante em um módulo específico.
    /// </summary>
    [HttpGet("ModuleProgress/Student/{studentId}/Module/{moduleId}")]
    [DefaultResponseTypes(typeof(BaseResponse<ModuleProgressResponse>))]
    public async Task<IActionResult> GetModuleProgress(
        [FromRoute] Guid moduleId,
        [FromRoute] Guid studentId,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new ModuleProgress(studentId, moduleId), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Cria um novo estudante.
    /// </summary>
    [HttpPost("Create")]
    [DefaultResponseTypes(typeof(BaseResponse<object>))]
    public async Task<IActionResult> Create([FromForm] CreateStudentRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Inscreve um estudante em um curso.
    /// </summary>
    [HttpPost("SubscribeCourse")]
    [DefaultResponseTypes(typeof(BaseResponse<SubscribeResponse>))]
    public async Task<IActionResult> SubscribeCourse([FromQuery] Subscribe request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Deleta um estudante pelo seu ID.
    /// </summary>
    [HttpDelete("Delete")]
    [DefaultResponseTypes(typeof(BaseResponse<object>))]
    public async Task<IActionResult> Delete([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteStudentRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}