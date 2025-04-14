using System.Linq.Expressions;
using Domain.Entities.Core;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Search;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITeacherRepository _teacherRepository;

    public Handler(
        ICourseRepository courseRepository,
        ICategoryRepository categoryRepository,
        ITeacherRepository teacherRepository)
    {
        _courseRepository = courseRepository;
        _categoryRepository = categoryRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return new BaseResponse(400, "O campo de pesquisa não pode estar vazio");

        var searchTerm = request.Query.ToLower();
        var page = request.Page ?? 0;
        var pageSize = request.PageSize ?? 10;
        var skip = page * pageSize;

        var courses = await _courseRepository.GetAllProjectedAsync(
            c => c.DeletedDate == null && c.Name.Name.ToLower().Contains(searchTerm) || c.Description.Text.ToLower().Contains(searchTerm),
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
            pageSize,
            c => c.Image
        );

        var categories = await _categoryRepository.GetAllProjectedAsync(
            c => c.DeletedDate == null && c.Name.Name.ToLower().Contains(searchTerm),
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
            pageSize,
            c => c.Image
        );

        var teachers = await _teacherRepository.GetAllProjectedAsync(
            t => t.DeletedDate == null && t.Name.Name.ToLower().Contains(searchTerm),
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
            pageSize,
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
            return name != null && name.ToLower().Contains(searchTerm) ? 1 : 0;
        });

        if (!orderedResults.Any())
            return new BaseResponse(404, "Nenhum resultado encontrado para a pesquisa", null, new List<object>());

        return new BaseResponse(200, "Resultados da pesquisa", null, orderedResults);
    }
}