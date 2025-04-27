using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Module.Delete;

/// <summary>
/// Handler responsável pela exclusão lógica de um módulo.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IModuleRepository _moduleRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMessageQueueService _messageQueueService;

    /// <summary>
    /// Construtor do handler de deleção do módulo.
    /// </summary>
    public Handler(
        IModuleRepository moduleRepository,
        IDbCommit dbCommit,
        IMessageQueueService messageQueueService)
    {
        _moduleRepository = moduleRepository;
        _dbCommit = dbCommit;
        _messageQueueService = messageQueueService;
    }

    /// <summary>
    /// Manipula a exclusão de um módulo.
    /// </summary>
    /// <param name="request">Request com o ID do módulo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse{object}"/> com status da operação</returns>
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca o módulo no repositório
        var moduleFound = await _moduleRepository.GetWithParametersAsyncWithTracking(
            x => x.Id == request.ModuleId,
            cancellationToken,
            x => x.Lectures,
            x => x.Lectures.Select(l => l.StudentLectures),
            x => x.Lectures.Select(l => l.Video)
        );

        // Verifica se o módulo foi encontrado
        if (moduleFound is null)
        {
            return new BaseResponse<object>(
                statusCode: 404,
                message: "Module not found"
            );
        }

        // Remove o módulo do repositório
        _moduleRepository.Delete(moduleFound);

        // Confirma as alterações no banco de dados
        await _dbCommit.Commit(cancellationToken);

        // Retorna sucesso
        return new BaseResponse<object>(
            statusCode: 200,
            message: "Module deleted successfully"
        );
    }
}