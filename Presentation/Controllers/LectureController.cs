using MediatR;
using Microsoft.AspNetCore.Mvc;
using CreateRequest = Application.UseCases.Lecture.Create.Request;
using DeleteRequest = Application.UseCases.Lecture.Delete.Request;

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
    public async Task<IActionResult> Create([FromBody] CreateRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response, response.notifications });
    }

    /// <summary>
    /// Remove uma aula com base no seu ID.
    /// </summary>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return StatusCode(response.statuscode, new { response.message, response.Response, response.notifications });
    }

    /// <summary>
    /// Retorna todas as aulas de um curso com base no estudante e paginação.
    /// </summary>
    [HttpGet("Get/All/{courseId}/{studentId}/Page/{page}/PageSize/{pageSize}")]
    public async Task<IActionResult> GetAll([FromRoute] Guid courseId, [FromRoute] Guid studentId,
        [FromRoute] int page, [FromRoute] int pageSize, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new Application.UseCases.Lecture.Get.AllCourseCompleted
            .Request(courseId, studentId, page, pageSize), cancellationToken);

        return StatusCode(response.statuscode, new { response.message, response.Response, response.notifications });
    }

    /// <summary>
    /// Verifica se uma aula foi concluída por um estudante.
    /// </summary>
    [HttpGet("Get/IsLectureCompleted/{studentId}/{lectureId}")]
    public async Task<IActionResult> IsLectureCompleted([FromRoute] Guid studentId, [FromRoute] Guid lectureId,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new Application.UseCases.Lecture.Get.IsLectureCompleted
            .Request(studentId, lectureId), cancellationToken);

        return StatusCode(response.statuscode, new { response.message, response.Response, response.notifications });
    }

    /// <summary>
    /// Marca uma aula como concluída para um estudante em um curso.
    /// </summary>
    [HttpPost("CompleteLecture/{courseId}/{studentId}/{lectureId}")]
    public async Task<IActionResult> CompleteLecture([FromRoute] Guid courseId, [FromRoute] Guid studentId,
        [FromRoute] Guid lectureId, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new Application.UseCases.Lecture.MarkLectureCompleted
            .Request(courseId, studentId, lectureId), cancellationToken);

        return StatusCode(response.statuscode, new { response.message, response.Response, response.notifications });
    }
}
