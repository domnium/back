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

/// <summary>
/// Controlador responsável por operações relacionadas a cursos (Course),
/// incluindo criação, consulta por categoria, IA, popularidade e exclusão.
/// </summary>
[ApiController]
[Route("Course")]
[Authorize]
public class CourseController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Cria um novo curso.
    /// </summary>
    /// <param name="request">Dados do curso a ser criado</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da criação</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateRequest request ,CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(request, cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.Response });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Retorna todos os cursos paginados.
    /// </summary>
    /// <param name="page">Número da página</param>
    /// <param name="pageSize">Quantidade de itens por página</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista paginada de cursos</returns>
    [HttpGet("Get/All")]
    public async Task<IActionResult> GetAll(int page, int pageSize, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetAllRequest(page, pageSize), cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.Response });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Retorna cursos filtrados por categoria com paginação.
    /// </summary>
    /// <param name="categoryId">ID da categoria</param>
    /// <param name="page">Página atual</param>
    /// <param name="pageSize">Quantidade de registros por página</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de cursos da categoria</returns>
    [HttpGet("Get/ByCategory/{categoryId}/Page/{page}/PageSize/{pageSize}")]
    public async Task<IActionResult> GetByCategory(Guid categoryId, int page, int pageSize, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetByCategory(categoryId, page, pageSize), cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.Response });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Retorna detalhes de um curso pelo ID.
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Detalhes do curso</returns>
    [HttpGet("Get/ById/{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetByIdRequest(id), cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.Response });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Retorna cursos associados a uma IA específica, com paginação.
    /// </summary>
    /// <param name="IAId">ID da IA</param>
    /// <param name="page">Página atual</param>
    /// <param name="pageSize">Quantidade de registros por página</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de cursos associados à IA</returns>
    [HttpGet("Get/ByIA/{IAId}/Page/{page}/PageSize/{pageSize}")]
    public async Task<IActionResult> GetByIA(Guid IAId, int page, int pageSize, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetByIA(IAId, page, pageSize), cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.Response });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Retorna os cursos mais populares.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de cursos mais acessados/inscritos</returns>
    [HttpGet("Get/MostPopular")]
    public async Task<IActionResult> GetMostPopular(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new GetMostPopular(), cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.Response });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }

    /// <summary>
    /// Exclui um curso pelo seu ID.
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação de exclusão</returns>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var response = await mediator.Send(new DeleteRequest(id), cancellationToken);
            return StatusCode(response.statuscode, new { response.message, response.Response });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.StackTrace);
        }
    }
}
