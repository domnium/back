using System.Linq.Expressions;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Module.GetAll;

/// <summary>
/// Handler responsável por retornar até 100 módulos do sistema.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly IModuleRepository _moduleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor para o handler de retorno de até 100 módulos do sistema.
    /// </summary>
    public Handler(IModuleRepository moduleRepository, IMapper mapper)
    {
        _moduleRepository = moduleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Manipula o retorno de até 100 módulos.
    /// </summary>
    /// <param name="request">Request vazio para o retorno de até 100 módulos do sistema</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        var modules = await _moduleRepository.GetAllProjectedAsync(
            x => x.DeletedDate == null,
            selector: x => x,
            cancellationToken: cancellationToken,
            skip: 0,
            take: 100,
            includes: new Expression<Func<Domain.Entities.Core.Module, object>>[] { x => x.Course }
        );

        if (modules is null || !modules.Any())
            return new BaseResponse<List<Response>>(404, "Modules not found");

        // Mapeia os módulos para o DTO de resposta
        var response = _mapper.Map<List<Response>>(modules);

        return new BaseResponse<List<Response>>(200, "Modules found", response);
    }
}