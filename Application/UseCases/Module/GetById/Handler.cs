using System.Linq.Expressions;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Module.GetById;

/// <summary>
/// Handler responsável por retornar apenas um módulo.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IModuleRepository _moduleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor para o handler de retorno de apenas um módulo.
    /// </summary>
    public Handler(IModuleRepository moduleRepository, IMapper mapper)
    {
        _moduleRepository = moduleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Manipula o retorno de apenas um módulo com base no identificador encaminhado.
    /// </summary>
    /// <param name="request">Request com o identificador de módulo para filtro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var module = await _moduleRepository.GetProjectedAsync(
            x => x.DeletedDate == null && x.Id == request.ModuleId,
            selector: x => x,
            cancellationToken: cancellationToken,
            includes: new Expression<Func<Domain.Entities.Core.Module, object>>[] { x => x.Course }
        );

        if (module is null)
            return new BaseResponse<Response>(404, "Module not found");

        // Mapeia o módulo para o DTO de resposta
        var response = _mapper.Map<Response>(module);

        return new BaseResponse<Response>(200, "Module found", response);
    }
}