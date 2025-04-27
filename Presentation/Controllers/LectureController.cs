using MediatR;
using Microsoft.AspNetCore.Mvc;
using CreateRequest = Application.UseCases.Lecture.Create.Request;
using DeleteRequest = Application.UseCases.Lecture.Delete.Request;
using GetAllRequest = Application.UseCases.Lecture.Get.AllCourseCompleted.Request;
using IsLectureCompletedRequest = Application.UseCases.Lecture.Get.IsLectureCompleted.Request;
using MarkLectureCompletedRequest = Application.UseCases.Lecture.MarkLectureCompleted.Request;

using GetAllResponse = Application.UseCases.Lecture.Get.AllCourseCompleted.Response;
using IsLectureCompletedResponse = Application.UseCases.Lecture.Get.IsLectureCompleted.Response;
using MarkLectureCompletedResponse = Application.UseCases.Lecture.MarkLectureCompleted;

using Domain.Records;
using Presentation.Common.Api.Attributes;

namespace Presentation.Controllers;

/// <summary>
/// Controlador responsável por gerenciar operações relacionadas a aulas (lectures),
/// como criação, exclusão, conclusão e consultas.
/// </summary>
[Route("Lecture")]
[ApiController]
public class LectureController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Cria uma nova aula (lecture).
    /// </summary>
    [HttpPost("Create")]
    [DefaultResponseTypes(typeof(BaseResponse<object>))]
    public async Task<IActionResult> Create([FromForm] CreateRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Remove uma aula com base no seu ID.
    /// </summary>
    [HttpDelete("Delete/{id}")]
    [DefaultResponseTypes(typeof(BaseResponse<object>))]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteRequest(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Retorna todas as aulas de um curso com base no estudante e paginação.
    /// </summary>
    [HttpGet("Get/All/{courseId}/{studentId}/Page/{page}/PageSize/{pageSize}")]
    [DefaultResponseTypes(typeof(BaseResponse<List<GetAllResponse>>))]
    public async Task<IActionResult> GetAll(
        [FromRoute] Guid courseId,
        [FromRoute] Guid studentId,
        [FromRoute] int page,
        [FromRoute] int pageSize,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllRequest(courseId, studentId, page, pageSize), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Verifica se uma aula foi concluída por um estudante.
    /// </summary>
    [HttpGet("Get/IsLectureCompleted/{studentId}/{lectureId}")]
    [DefaultResponseTypes(typeof(BaseResponse<IsLectureCompletedResponse>))]
    public async Task<IActionResult> IsLectureCompleted(
        [FromRoute] Guid studentId,
        [FromRoute] Guid lectureId,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new IsLectureCompletedRequest(studentId, lectureId), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Marca uma aula como concluída para um estudante em um curso.
    /// </summary>
    [HttpPost("CompleteLecture")]
    [DefaultResponseTypes(typeof(BaseResponse<object>))]
    public async Task<IActionResult> CompleteLecture(
        [FromQuery] Guid courseId,
        [FromQuery] Guid studentId,
        [FromQuery] Guid lectureId,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new MarkLectureCompletedRequest(courseId, studentId, lectureId), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}