using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Category.GetById;

public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public Handler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Verifica se o ID foi fornecido
        if (request.Id is null)
        {
            return new BaseResponse<Response>(
                statusCode: 400,
                message: "Category ID is required"
            );
        }

        // Busca a categoria no repositório
        var entity = await _categoryRepository.GetWithParametersAsync(
            x => x.Id.Equals(request.Id), cancellationToken);

        // Verifica se a categoria foi encontrada
        if (entity is null)
        {
            return new BaseResponse<Response>(
                statusCode: 404,
                message: "Category not found"
            );
        }

        // Mapeia a entidade para o DTO de resposta
        var responseDto = _mapper.Map<Response>(entity);
        // Retorna o BaseResponse com os dados
        return new BaseResponse<Response>(
            statusCode: 200,
            message: "Category found",
            response: responseDto
        );
    }
}