using System.Linq.Expressions;
using Domain.Entities.Core;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Search;

/// <summary>
/// Handler responsável por processar a requisição de busca combinada em cursos, categorias e professores.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITeacherRepository _teacherRepository;

    /// <summary>
    /// Inicializa uma nova instância do handler de busca.
    /// </summary>
    /// <param name="courseRepository">Repositório de cursos.</param>
    /// <param name="categoryRepository">Repositório de categorias.</param>
    /// <param name="teacherRepository">Repositório de professores.</param>    
    public Handler(
        ICourseRepository courseRepository,
        ICategoryRepository categoryRepository,
        ITeacherRepository teacherRepository)
    {
        _courseRepository = courseRepository;
        _categoryRepository = categoryRepository;
        _teacherRepository = teacherRepository;
    }

    /// <summary>
    /// Manipula a requisição de busca, retornando resultados combinados de cursos, categorias e professores.
    /// </summary>
    /// <param name="request">Requisição contendo o termo de busca e parâmetros de paginação.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return new BaseResponse(400, "O campo de pesquisa não pode estar vazio");

        var skip = request.Page * request.PageSize;
        var take = request.PageSize;

        var courses = await _courseRepository.GetAllProjectedAsync(
            c => c.DeletedDate == null && 
                (c.Name.Name.ToLower().Contains(request.Query) || 
                 c.Description.Text.ToLower().Contains(request.Query)),
            c => new
            {
                Type = "course",
                c.Id,
                Name = c.Name.Name,
                Description = c.Description.Text,
                ImageUrl = c.Image != null && c.Image.UrlTemp != null ? c.Image.UrlTemp.Endereco : null,
                c.Price
            },
            cancellationToken,
            skip,
            take,
            c => c.Image
        );

        var categories = await _categoryRepository.GetAllProjectedAsync(
            c => c.DeletedDate == null && c.Name.Name.ToLower().Contains(request.Query),
            c => new
            {
                Type = "category",
                c.Id,
                Name = c.Name.Name,
                Description = (string?)null,
                ImageUrl = c.Image != null && c.Image.UrlTemp != null ? c.Image.UrlTemp.Endereco : null,
                Price = (decimal?)null
            },
            cancellationToken,
            skip,
            take,
            c => c.Image
        );

        var teachers = await _teacherRepository.GetAllProjectedAsync(
            t => t.DeletedDate == null && t.Name.Name.ToLower().Contains(request.Query),
            t => new
            {
                Type = "teacher",
                t.Id,
                Name = t.Name.Name,
                Description = t.Description.Text,
                ImageUrl = t.Picture != null && t.Picture.UrlTemp != null ? t.Picture.UrlTemp.Endereco : null,
                Price = (decimal?)null
            },
            cancellationToken,
            skip,
            take,
            t => t.Picture
        );

        var combinedResults = new List<object>();
        combinedResults.AddRange(courses);
        combinedResults.AddRange(categories);
        combinedResults.AddRange(teachers);

        var orderedResults = combinedResults.OrderByDescending(r =>
        {
            var property = r?.GetType().GetProperty("Name");
            var name = property != null ? property.GetValue(r, null) as string : null;
            return name != null && name.Contains(request.Query, StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        });

        if (!orderedResults.Any())
            return new BaseResponse(404, "Nenhum resultado encontrado para a pesquisa", null, new List<object>());

        return new BaseResponse(200, "Resultados da pesquisa", null, orderedResults);
    }
}