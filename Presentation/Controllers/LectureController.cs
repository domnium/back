using MediatR;
using Microsoft.AspNetCore.Http;
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
    /// <param name="request">Dados da aula a ser criada</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da operação e dados da aula criada</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response, response.notifications});
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Remove uma aula com base no seu ID.
    /// </summary>
    /// <param name="request">Request contendo o ID da aula</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da operação</returns>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(DeleteRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response, response.notifications});
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Retorna todas as aulas de um curso com base no estudante e paginação.
    /// </summary>
    /// <param name="courseId">ID do curso</param>
    /// <param name="studentId">ID do estudante</param>
    /// <param name="page">Página atual da listagem</param>
    /// <param name="pageSize">Quantidade de registros por página</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da operação e lista de aulas</returns>
    [HttpGet("Get/All/{courseId}/{studentId}/Page/{page}/PageSize/{pageSize}")]
    public async Task<IActionResult> GetAll(Guid courseId, Guid studentId, int page, int pageSize, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new Application.UseCases.Lecture.Get.AllCourseCompleted
                .Request(courseId, studentId, page, pageSize), cancellationToken);

            return StatusCode(response.statuscode, new {response.message, response.Response, response.notifications});
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Verifica se uma aula foi concluída por um estudante.
    /// </summary>
    /// <param name="studentId">ID do estudante</param>
    /// <param name="lectureId">ID da aula</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da operação e resultado booleano</returns>
    [HttpGet("Get/IsLectureCompleted/{studentId}/{lectureId}")]
    public async Task<IActionResult> IsLectureCompleted(Guid studentId, Guid lectureId, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new Application.UseCases.Lecture.Get.IsLectureCompleted.
                Request(studentId, lectureId), cancellationToken);

            return StatusCode(response.statuscode, new {response.message, response.Response, response.notifications});
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Marca uma aula como concluída para um estudante em um curso.
    /// </summary>
    /// <param name="courseId">ID do curso</param>
    /// <param name="studentId">ID do estudante</param>
    /// <param name="lectureId">ID da aula</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status da operação</returns>
    [HttpPost("CompleteLecture/{courseId}/{studentId}/{lectureId}")]
    public async Task<IActionResult> CompleteLecture(Guid courseId, Guid studentId,
        Guid lectureId, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new Application.UseCases.Lecture.MarkLectureCompleted
                .Request(courseId, studentId, lectureId), cancellationToken);
            return StatusCode(response.statuscode, new {response.message, response.Response, response.notifications});
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }
}
