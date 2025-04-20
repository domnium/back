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

/// <summary>
/// Handler responsável pela criação de uma nova categoria no sistema,
/// incluindo o processamento de imagem associada.
///
/// O fluxo contempla:
/// - Verificação de existência da categoria por nome.
/// - Criação da entidade <see cref="Category"/> com a imagem.
/// - Armazenamento temporário do arquivo no disco.
/// - Envio de mensagem assíncrona para upload definitivo via RabbitMQ.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMessageQueueService _messageQueueService;
    private readonly ITemporaryStorageService _temporaryStorageService;

    /// <summary>
    /// Construtor do handler de criação de categoria.
    /// </summary>
    /// <param name="categoryRepository">Repositório de categorias</param>
    /// <param name="dbCommit">Serviço para commit da unidade de trabalho</param>
    /// <param name="messageQueueService">Serviço para envio de mensagens assíncronas</param>
    /// <param name="temporaryStorageService">Serviço de armazenamento temporário de arquivos</param>
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

    /// <summary>
    /// Manipula a criação de uma nova categoria, salvando os metadados e processando a imagem em paralelo.
    /// </summary>
    /// <param name="request">Request contendo nome, descrição e imagem da categoria</param>
    /// <param name="cancellationToken">Token de cancelamento da operação</param>
    /// <returns>Retorna um <see cref="BaseResponse"/> com status HTTP, mensagem e notificações de erro (se houver)</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        // Verifica se categoria já existe
        if (await _categoryRepository.GetWithParametersAsync(
            x => x.Name.Name.Equals(request.Name), cancellationToken) is not null)
            return new BaseResponse(400, "Category already exists");

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

        if (!newCategory.IsValid)
            return new BaseResponse(400, "Error creating category", newCategory.Notifications.ToList());

        await _categoryRepository.CreateAsync(newCategory, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        // Salva a imagem fisicamente em diretório temporário
        var tempPath = await _temporaryStorageService.SaveAsync(
            newCategory.Image.TemporaryPath!.Body!,
            newCategory.Image.Id,
            request.Imagem.OpenReadStream(),
            cancellationToken
        );

        // Enfileira upload da imagem para o serviço de background
        await _messageQueueService.EnqueueUploadMessageAsync(
            new UploadFileMessage(
                newCategory.Image.Id,
                Configuration.BucketArchives,
                newCategory.Image.TemporaryPath.Body!,
                request.Imagem.ContentType,
                tempPath
            ),
            cancellationToken
        );

        return new BaseResponse(201, "Category created");
    }
}
