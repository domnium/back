using System;
using Domain;
using Domain.Entities;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Category.Create;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IAwsService _awsService;
    private readonly IPictureRepository _pictureRepository;

    public Handler(ICategoryRepository categoryRepository,
     IDbCommit dbCommit,
     IPictureRepository pictureRepository, IAwsService awsService)
    {
        _categoryRepository = categoryRepository;
        _dbCommit = dbCommit;
        _pictureRepository = pictureRepository;
        _awsService = awsService;
    }


    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        if( await _categoryRepository
            .GetWithParametersAsync(x => x.Name.Name.Equals(request.Name),
             cancellationToken) is not null) return new BaseResponse(400, "Category already exists");

        var pictureId = Guid.NewGuid();

        var awsKey = await _awsService.UploadFileAsync(
            Configuration.BucketArchives,
            pictureId.ToString(),
            request.Imagem.OpenReadStream(),
            request.Imagem.ContentType
        ); 

        if(awsKey is null) return new BaseResponse(400, "Error uploading file to AWS S3");

        var newPicture = new Picture(new BigString(awsKey), true, new AppFile(request.Imagem.OpenReadStream(), "FotoCategoria"));
        newPicture.SetGuid(pictureId);
        newPicture.SetBucket(Configuration.BucketArchives);
        newPicture.SetTemporaryUrl(new Url(awsKey), DateTime.UtcNow.AddDays(1));
        
        if (newPicture is null || newPicture.Notifications.Any()) return new BaseResponse(400, "Error creating picture"
            + newPicture.Notifications.Select(x => x.Message).ToString());

        var storedPicture = await _pictureRepository.CreateReturnEntity(newPicture, cancellationToken);
    
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
