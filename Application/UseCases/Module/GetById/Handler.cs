using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Module.GetById;

/// <summary>
/// Handler responsável por retornar apenas um módulo.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IModuleRepository _moduleRepository;

    /// <summary>
    /// Construtor para o handler de retorno de apenas um módulo.
    /// </summary>
    public Handler(IModuleRepository moduleRepository)
    {
        _moduleRepository = moduleRepository;
    }

    /// <summary>
    /// Manipula o retorno de apenas um módulo com base no identificador encaminhado.
    /// </summary>
    /// <param name="request">Request com o identificador de módulo para filtro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var module = await _moduleRepository.GetProjectedAsync(
            x => x.DeletedDate != null && x.Id == request.ModuleId, 
            x => new {
                x.Id,
                x.Name,
                x.Description,
                x.Course
            }
            ,cancellationToken, x => x.Course
        );

        if(module is null) return new BaseResponse(404, "Module not found");
        return new BaseResponse(200, "Module found", null, module);
    }
}
