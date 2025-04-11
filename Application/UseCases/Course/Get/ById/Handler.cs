using System;
using System.Linq.Expressions;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Get.ById;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICourseRepository _courseRepository;
    public Handler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        if (request.id is null || request.id == Guid.Empty)
            return new BaseResponse(400, "Invalid course id");

        var course = await _courseRepository.GetProjectedAsync(
            filter: x => x.Id.Equals(request.id) && x.DeletedDate == null,
            selector: x => new {
                x.Id,
                x.Name,
                x.Subscribes,
                x.Description,
                x.AboutDescription,
                ImageUrl = x.Image!.UrlTemp!.Endereco!,
                TrailerUrl = x.Trailer!.UrlTemp!.Endereco!,
                TeacherName = x.Teacher!.Name!.Name!,
                TeacherPictureUrl = x.Teacher.Picture!.UrlTemp!.Endereco!
            },
            cancellationToken: cancellationToken,
            includes: new Expression<Func<Domain.Entities.Core.Course, object>>[] {
                x => x.Image,
                x => x.Trailer,
                x => x.Teacher,
                x => x.Teacher.Picture
            }
        );

        if (course is null) return new BaseResponse(404, "Course not found");
        return new BaseResponse(200, "Course found", null, course);
    }
}

