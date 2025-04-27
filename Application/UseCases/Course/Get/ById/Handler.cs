using System.Linq.Expressions;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Get.ById;

public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public Handler(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        if (request.id is null || request.id == Guid.Empty)
            return new BaseResponse<Response>(400, "Invalid course id");

        // Busca o curso no repositório
        var course = await _courseRepository.GetProjectedAsync(
            filter: x => x.Id.Equals(request.id) && x.DeletedDate == null,
            selector: x => x,
            cancellationToken: cancellationToken,
            includes: new Expression<Func<Domain.Entities.Core.Course, object>>[] {
                x => x.Picture,
                x => x.Trailer,
                x => x.Teacher,
                x => x.Teacher.Picture
            }
        );

        // Verifica se o curso foi encontrado
        if (course is null)
            return new BaseResponse<Response>(404, "Course not found");

        // Mapeia o curso para o DTO de resposta
        var response = _mapper.Map<Response>(course);

        // Retorna sucesso com o curso mapeado
        return new BaseResponse<Response>(200, "Course found", response);
    }
}