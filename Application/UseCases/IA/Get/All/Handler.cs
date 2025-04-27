using System.Linq.Expressions;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.IA.Get.All;

public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly IIARepository _iARepository;
    private readonly IMapper _mapper;

    public Handler(IIARepository iARepository, IMapper mapper)
    {
        _iARepository = iARepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca as IAs no repositório
        var iaList = await _iARepository.GetAllProjectedAsync(
            x => x.DeletedDate == null,
            selector: x => x,
            cancellationToken: cancellationToken,
            skip: request.skip,
            take: request.take,
            includes: new Expression<Func<Domain.Entities.Core.IA, object>>[] {
                x => x.Picture
            }
        );

        // Verifica se as IAs foram encontradas
        if (iaList is null || !iaList.Any())
            return new BaseResponse<List<Response>>(404, "IA not found");

        // Mapeia as IAs para o DTO de resposta
        var response = _mapper.Map<List<Response>>(iaList);

        // Retorna sucesso com as IAs mapeadas
        return new BaseResponse<List<Response>>(200, "IA found", response);
    }
}