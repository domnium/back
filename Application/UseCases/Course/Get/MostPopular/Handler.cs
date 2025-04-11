using System;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Get.MostPopular;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICourseRepository _courseRepository;

    public Handler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.TopFiveMostPopular(cancellationToken);
        if (courses is null || courses.Count == 0)
            return new BaseResponse(404, "Nenhum curso encontrado.");
            
        return new BaseResponse(200, null, null, courses);
    }
}
