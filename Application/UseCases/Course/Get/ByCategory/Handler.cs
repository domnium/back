using System.Linq.Expressions;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Get.ByCategory;

public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public Handler(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca os cursos no repositório
        var courses = await _courseRepository.GetAllProjectedAsync(
            x => x.DeletedDate == null && x.CategoryId.Equals(request.CategoryId),
            selector: x => x,
            cancellationToken: cancellationToken,
            skip: request.page ?? 0,
            take: request.pageSize ?? 100,
            includes: new Expression<Func<Domain.Entities.Core.Course, object>>[] {
                x => x.Picture,
                x => x.Trailer,
                x => x.Teacher,
                x => x.Teacher.Picture
            }
        );

        // Verifica se os cursos foram encontrados
        if (courses is null || !courses.Any())
            return new BaseResponse<List<Response>>(404, "Courses not found");

        // Mapeia os cursos para o DTO de resposta
        var response = _mapper.Map<List<Response>>(courses);

        // Retorna sucesso com os cursos mapeados
        return new BaseResponse<List<Response>>(200, "Courses found", response);
    }
}