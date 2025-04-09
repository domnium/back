using System;
using Domain;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Category.Create;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IPictureRepository _pictureRepository;
    public Handler(ICategoryRepository categoryRepository,
     IDbCommit dbCommit,
     IPictureRepository pictureRepository)
    {
        _categoryRepository = categoryRepository;
        _dbCommit = dbCommit;
        _pictureRepository = pictureRepository;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        if( await _categoryRepository
            .GetWithParametersAsync(x => x.Name.Name.Equals(request.Name),
             cancellationToken) is not null) return new BaseResponse(400, "Category already exists");

        var newPicture = await _pictureRepository.CreateReturnEntity(new Picture(new BigString(awsKey),
             true, new AppFile(request.Imagem, "FotoCategoria")), cancellationToken);

        var newCategory = new Domain.Entities.Core.Category(
            new UniqueName(request.Name),
            new Description(request.Description),
            newPicture
        );

        await _categoryRepository.CreateAsync(newCategory, cancellationToken);
        await _dbCommit.Commit(cancellationToken);
        return new BaseResponse(201, "Category Created");
    }
}
