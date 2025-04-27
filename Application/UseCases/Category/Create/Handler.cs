using System;
using Domain;
using Domain.Entities;
using Domain.ExtensionsMethods;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Category.Create;

public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMessageQueueService _messageQueueService;
    private readonly ITemporaryStorageService _temporaryStorageService;

    public Handler(
        ICategoryRepository categoryRepository,
        IDbCommit dbCommit,
        IMessageQueueService messageQueueService,
        ITemporaryStorageService temporaryStorageService)
    {
        _categoryRepository = categoryRepository;
        _dbCommit = dbCommit;
        _messageQueueService = messageQueueService;
        _temporaryStorageService = temporaryStorageService;
    }

    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Verifica se a categoria já existe
        if (await _categoryRepository.GetWithParametersAsync(
            x => x.Name.Name.Equals(request.Name), cancellationToken) is not null)
        {
            return new BaseResponse<object>(
                statusCode: 400,
                message: "Category already exists"
            );
        }

        // Cria imagem temporária associada à categoria
        var newPicture = new Picture(
            new BigString(Configuration.PicturesCategoriesPath),
            true,
            new AppFile(request.Imagem.OpenReadStream(), request.Imagem.FileName),
            new BigString(Configuration.PicturesCategoriesPath),
            ContentTypeExtensions.ParseMimeType(request.Imagem.ContentType)
        );

        // Cria a categoria
        var newCategory = new Domain.Entities.Core.Category(
            new UniqueName(request.Name),
            new Description(request.Description),
            newPicture
        );

        // Verifica se a categoria é válida
        if (!newCategory.IsValid)
        {
            return new BaseResponse<object>(
                statusCode: 400,
                message: "Error creating category",
                notifications: newCategory.Notifications.ToList()
            );
        }

        // Define o proprietário da imagem
        newPicture.SetPictureOwner(newCategory);
        // Salva a categoria no banco de dados
        await _categoryRepository.CreateAsync(newCategory, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        // Salva a imagem fisicamente em diretório temporário
        var tempPath = await _temporaryStorageService.SaveAsync(
            newCategory.Picture.TemporaryPath!.Body!,
            newCategory.Picture.Id,
            request.Imagem.OpenReadStream(),
            cancellationToken
        );

        // Enfileira upload da imagem para o serviço de background
        await _messageQueueService.EnqueueUploadMessageAsync(
            new UploadFileMessage(
                newCategory.Picture.Id,
                Configuration.BucketArchives,
                newCategory.Picture.TemporaryPath.Body!,
                request.Imagem.ContentType,
                tempPath
            ),
            cancellationToken
        );

        // Retorna sucesso
        return new BaseResponse<object>(
            statusCode: 201,
            message: "Category created successfully"
        );
    }
}