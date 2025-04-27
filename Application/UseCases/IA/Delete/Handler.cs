using System;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.IA.Delete;

public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IIARepository _iaRepository;
    private readonly IDbCommit _dbCommit;   
    private readonly IMessageQueueService _messageQueueService;

    public Handler(
        IIARepository iaRepository, 
        IDbCommit dbCommit,
        IMessageQueueService messageQueueService)
    {
        _iaRepository = iaRepository;
        _dbCommit = dbCommit;
        _messageQueueService = messageQueueService;
    }

    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca a IA no repositório
        var ia = await _iaRepository.GetWithParametersAsync(
            c => c.Id.Equals(request.Id), cancellationToken);

        // Verifica se a IA foi encontrada
        if (ia is null)
        {
            return new BaseResponse<object>(
                statusCode: 404,
                message: "IA not found"
            );
        }

        // Enfileira a exclusão da imagem, se existir
        if (ia.Picture?.AwsKey is not null && !string.IsNullOrWhiteSpace(ia.Picture.BucketName))
        {
            await _messageQueueService.EnqueueDeleteMessageAsync(
                new DeleteFileMessage(ia.Picture.BucketName, ia.Picture.AwsKey.Body),
                cancellationToken
            );
        }

        // Remove a IA do repositório
        _iaRepository.Delete(ia);

        // Confirma as alterações no banco de dados
        await _dbCommit.Commit(cancellationToken);

        // Retorna sucesso
        return new BaseResponse<object>(
            statusCode: 200,
            message: "IA deleted successfully"
        );
    }
}