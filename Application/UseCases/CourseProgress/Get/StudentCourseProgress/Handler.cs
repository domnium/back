using System;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.CourseProgress.Get.StudentCourseProgress;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICourseProgressRepository _courseProgressRepository;
    public Handler(ICourseProgressRepository courseProgressRepository)
    {
        _courseProgressRepository = courseProgressRepository;
    }
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var courseProgress = await _courseProgressRepository.GetCourseProgress(request.StudentId, request.CourseId);
        if (courseProgress.Equals(0))
            return new BaseResponse(404, "Não existe esse estudante ou módulo.");
        return new BaseResponse(200, null, null,courseProgress);
    }
}

