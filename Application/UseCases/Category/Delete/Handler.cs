using System;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.Delete;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDbCommit _dbCommit;   
    public Handler(ICategoryRepository categoryRepository, IDbCommit dbCommit)
    {
        _categoryRepository = categoryRepository;
        _dbCommit = dbCommit;
    }
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetWithParametersAsync(
                c => c.Id.Equals(request.Id), cancellationToken);

        if (category is null)
            return new BaseResponse(404, "Category not found");
        
        await _categoryRepository.DeleteAsync(category, cancellationToken);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse(200, "Category deleted", null, category);
    }
}