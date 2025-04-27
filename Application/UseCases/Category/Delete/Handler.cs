using System;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.Delete;

public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDbCommit _dbCommit;   
    private readonly IMessageQueueService _messageQueueService;

    public Handler(ICategoryRepository categoryRepository, 
        IDbCommit dbCommit,
        IMessageQueueService messageQueueService)
    {
        _categoryRepository = categoryRepository;
        _dbCommit = dbCommit;
        _messageQueueService = messageQueueService;
    }

    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca a categoria no repositório
        var category = await _categoryRepository.GetWithParametersAsync(
            c => c.Id.Equals(request.Id), cancellationToken);

        // Verifica se a categoria foi encontrada
        if (category is null)
        {
            return new BaseResponse<object>(
                statusCode: 404,
                message: "Category not found"
            );
        }

        // Enfileira a exclusão da imagem, se existir
        if (category.Picture?.AwsKey is not null && !string.IsNullOrWhiteSpace(category.Picture.BucketName))
        {
            await _messageQueueService.EnqueueDeleteMessageAsync(
                new DeleteFileMessage(category.Picture.BucketName, category.Picture.AwsKey.Body),
                cancellationToken);
        }

        // Remove a categoria do repositório
        _categoryRepository.Delete(category);

        // Confirma as alterações no banco de dados
        await _dbCommit.Commit(cancellationToken);

        // Retorna sucesso
        return new BaseResponse<object>(
            statusCode: 200,
            message: "Category deleted successfully"
        );
    }
}