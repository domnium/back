using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Search;

/// <summary>
/// Handler responsável por processar a requisição de busca combinada em cursos, categorias e professores.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITeacherRepository _teacherRepository;

    /// <summary>
    /// Inicializa uma nova instância do handler de busca.
    /// </summary>
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
    public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return new BaseResponse<List<Response>>(400, "O campo de pesquisa não pode estar vazio");

        var skip = request.Page * request.PageSize;
        var take = request.PageSize;

        var courses = await _courseRepository.GetAllProjectedAsync(
            c => c.DeletedDate == null &&
                (c.Name.Name.ToLower().Contains(request.Query) ||
                 c.Description.Text.ToLower().Contains(request.Query)),
            c => new Response(
                "course",
                c.Id,
                c.Name.Name,
                c.Description.Text,
                c.Picture != null && c.Picture.UrlTemp != null ? c.Picture.UrlTemp.Endereco : null,
                c.Price
            ),
            cancellationToken,
            skip,
            take,
            c => c.Picture
        );

        var categories = await _categoryRepository.GetAllProjectedAsync(
            c => c.DeletedDate == null && c.Name.Name.ToLower().Contains(request.Query),
            c => new Response(
                "category",
                c.Id,
                c.Name.Name,
                null,
                c.Picture != null && c.Picture.UrlTemp != null ? c.Picture.UrlTemp.Endereco : null,
                null
            ),
            cancellationToken,
            skip,
            take,
            c => c.Picture
        );

        var teachers = await _teacherRepository.GetAllProjectedAsync(
            t => t.DeletedDate == null && t.Name.Name.ToLower().Contains(request.Query),
            t => new Response(
                "teacher",
                t.Id,
                t.Name.Name,
                t.Description.Text,
                t.Picture != null && t.Picture.UrlTemp != null ? t.Picture.UrlTemp.Endereco : null,
                null
            ),
            cancellationToken,
            skip,
            take,
            t => t.Picture
        );

        var combinedResults = courses.Concat(categories).Concat(teachers).ToList();

        if (!combinedResults.Any())
            return new BaseResponse<List<Response>>(404, "Nenhum resultado encontrado para a pesquisa");

        return new BaseResponse<List<Response>>(200, "Resultados da pesquisa", combinedResults);
    }
}