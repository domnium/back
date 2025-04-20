using System;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.GetAll;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    public Handler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
       var entities = await _categoryRepository.GetAllWithParametersAsync(
            x => x.DeletedDate == null, cancellationToken, 0, 100, x => x.Picture);

        var categories = entities.Select(x => new {
            x.Id,
            x.Name,
            x.Description,
            ImageUrl = x.Picture?.UrlTemp
        }).ToList();
        
        if(categories is null || categories.Any()) return new BaseResponse(404, "Categories not found");
        return new BaseResponse(200, "Categories found", null, categories);
    }
}
