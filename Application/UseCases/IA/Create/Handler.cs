using System;
using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.IA.Create;

public class Handler(IIARepository iARepository, 
    IDbCommit dbCommit,
    IMessageQueueService messageQueueService,
    ITemporaryStorageService temporaryStorageService) : IRequestHandler<Request, BaseResponse>
{
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var iaAlreadyExists = await iARepository.GetWithParametersAsync(
            i => i.Name!.Name.Equals(request.Name), cancellationToken
        );

        if(iaAlreadyExists is not null)
            return new BaseResponse(400, "IA already exists");
        
        var newIa = new Domain.Entities.Core.IA(
            new UniqueName(request.Name),
            new Domain.Entities.Picture(
                null, true, new AppFile(
                    request.Picture.OpenReadStream(), 
                    request.Picture.FileName
                ), new BigString(Configuration.PicturesIAPath)
            )
        );

        if(!newIa.IsValid)
            return new BaseResponse(400, "Request invalid", newIa.Notifications.ToList());
        
        await iARepository.CreateAsync(newIa, cancellationToken);
        var tempPath = await temporaryStorageService.SaveAsync(
            Configuration.PicturesIAPath,
            newIa.Picture.Id,
            request.Picture.OpenReadStream(),
            cancellationToken
        );

        // Enfileira imagem para upload
        await messageQueueService.EnqueueUploadMessageAsync(new UploadFileMessage(
            newIa.Picture.Id,
            Configuration.BucketArchives,
            Configuration.PicturesIAPath,
            request.Picture.ContentType,
            tempPath
        ), cancellationToken);

        await dbCommit.Commit(cancellationToken);
        return new BaseResponse(201, "IA created Succesfully");
    }
}
