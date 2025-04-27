using System;
using Domain;
using Domain.ExtensionsMethods;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.IA.Create;

public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IIARepository _iARepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMessageQueueService _messageQueueService;
    private readonly ITemporaryStorageService _temporaryStorageService;

    public Handler(
        IIARepository iARepository,
        IDbCommit dbCommit,
        IMessageQueueService messageQueueService,
        ITemporaryStorageService temporaryStorageService)
    {
        _iARepository = iARepository;
        _dbCommit = dbCommit;
        _messageQueueService = messageQueueService;
        _temporaryStorageService = temporaryStorageService;
    }

    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Verifica se a IA já existe
        var iaAlreadyExists = await _iARepository.GetWithParametersAsync(
            i => i.Name!.Name.Equals(request.Name), cancellationToken
        );

        if (iaAlreadyExists is not null)
        {
            return new BaseResponse<object>(
                statusCode: 400,
                message: "IA already exists"
            );
        }

        // Cria a nova IA
        var newIa = new Domain.Entities.Core.IA(
            new UniqueName(request.Name),
            new Domain.Entities.Picture(
                null,
                true,
                new AppFile(
                    request.Picture.OpenReadStream(),
                    request.Picture.FileName
                ),
                new BigString(Configuration.PicturesIAPath),
                ContentTypeExtensions.ParseMimeType(request.Picture.ContentType)
            )
        );

        // Verifica se a IA é válida
        if (!newIa.IsValid)
        {
            return new BaseResponse<object>(
                statusCode: 400,
                message: "Request invalid",
                notifications: newIa.Notifications.ToList()
            );
        }

        // Salva a IA no repositório
        await _iARepository.CreateAsync(newIa, cancellationToken);

        // Salva a imagem temporariamente
        var tempPath = await _temporaryStorageService.SaveAsync(
            Configuration.PicturesIAPath,
            newIa.Picture!.Id,
            request.Picture.OpenReadStream(),
            cancellationToken
        );

        // Enfileira a imagem para upload
        await _messageQueueService.EnqueueUploadMessageAsync(
            new UploadFileMessage(
                newIa.Picture.Id,
                Configuration.BucketArchives,
                Configuration.PicturesIAPath,
                request.Picture.ContentType,
                tempPath
            ),
            cancellationToken
        );

        // Confirma as alterações no banco de dados
        await _dbCommit.Commit(cancellationToken);

        // Retorna sucesso
        return new BaseResponse<object>(
            statusCode: 201,
            message: "IA created successfully"
        );
    }
}