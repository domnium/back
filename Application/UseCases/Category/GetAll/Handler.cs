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
        var categories = await _categoryRepository.GetAllProjectedAsync(
            x => x.DeletedDate != null, 
            x => new {
                x.Id,
                x.Name,
                x.Description,
                x.Image.UrlTemp
            }
            ,cancellationToken, 0, 100, x => x.Image
        );
        
        if(categories is null) return new BaseResponse(404, "Categories not found");
        return new BaseResponse(200, "Categories found", null, categories);
    }
}
