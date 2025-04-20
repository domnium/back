using System;
using System.Linq.Expressions;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Course.Get.ByCategory;

public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly ICourseRepository _courseRepository;
    public Handler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetAllProjectedAsync(
            x => x.DeletedDate == null && x.CategoryId.Equals(request.CategoryId),
            selector: x => new {
                x.Id,
                x.Name,
                x.Subscribes,
                x.Description,
                x.AboutDescription,
                ImageUrl = x.Picture!.UrlTemp!.Endereco!,
                TrailerUrl = x.Trailer!.UrlTemp!.Endereco!,
                TeacherName = x.Teacher!.Name!.Name!,
                TeacherPictureUrl = x.Teacher.Picture!.UrlTemp!.Endereco!
            },
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

        if (courses is null) return new BaseResponse(404, "Courses not found");
        return new BaseResponse(200, "Courses found", null, courses);
    }
}
