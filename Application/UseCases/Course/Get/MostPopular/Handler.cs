using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Get.MostPopular;

public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public Handler(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<List<Response>>> Handle(Request request, 
        CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.TopFiveMostPopular(cancellationToken);
        if (courses is null || courses.Count == 0)
            return new BaseResponse<List<Response>>(404, "No courses found.");

        var response = _mapper.Map<List<Response>>(courses);
        return new BaseResponse<List<Response>>(200, "Courses found", response);
    }
}