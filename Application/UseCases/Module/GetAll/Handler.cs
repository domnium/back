using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Module.GetAll;

/// <summary>
/// Handler responsável por retornar até 100 módulos do sistema.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IModuleRepository _moduleRepository;

    /// <summary>
    /// Construtor para o handler de retorno de até 100 módulos do sistema.
    /// </summary>
    public Handler(IModuleRepository moduleRepository)
    {
        _moduleRepository = moduleRepository;
    }

    /// <summary>
    /// Manipula o retorno de até 100 módulos.
    /// </summary>
    /// <param name="request">Request vazio para o retorno de até 100 módulos do sistema</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var modules = await _moduleRepository.GetAllProjectedAsync(
            x => x.DeletedDate != null, 
            x => new {
                x.Id,
                x.Name,
                x.Description,
                x.Course
            }
            ,cancellationToken, 0, 100, x => x.Course
        );

        if(modules is null) return new BaseResponse(404, "Modules not found");
        return new BaseResponse(200, "Modules found", null, modules);
    }
}
