using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.GetAll;

public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public Handler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca as categorias no repositório
        var entities = await _categoryRepository.GetAllWithParametersAsync(
            x => x.DeletedDate == null,
            cancellationToken,
            skip: request.Skip,
            take: request.Take,
            includes: x => x.Picture
        );

        if (entities is null || !entities.Any())
        {
            return new BaseResponse<List<Response>>(
                statusCode: 404,
                message: "Categories not found"
            );
        }

        // Mapeia as entidades para o DTO de resposta
        var responseDtos = _mapper.Map<List<Response>>(entities);
        // Retorna o BaseResponse com os dados
        return new BaseResponse<List<Response>>(
            statusCode: 200,
            message: "Categories found",
            response: responseDtos
        );
    }
}