using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Module.Delete;

/// <summary>
/// Handler responsável pela exclusão lógica de um módulo.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IModuleRepository _moduleRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMessageQueueService _messageQueueService;

    /// <summary>
    /// Construtor do handler de deleção do módulo.
    /// </summary>
    public Handler(IModuleRepository moduleRepository,
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
    /// <returns><see cref="BaseResponse"/> com status da operação</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var moduleFound = await _moduleRepository
            .GetWithParametersAsync(x => x.Id == request.ModuleId, cancellationToken);

        if (moduleFound is null)
            return new BaseResponse(404, "Module not found");

        await _moduleRepository.DeleteAsync(moduleFound, cancellationToken);

        await _dbCommit.Commit(cancellationToken);

        return new BaseResponse(200, "Module deleted successfully");
    }
}
