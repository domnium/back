using Domain.Records;
using MediatR;

namespace Application.UseCases.Lecture.Get.AllCourseCompleted;

public record Request(
    Guid StudentId,
    Guid CourseId,
    int skip, 
    int take
) 
: IRequest<BaseResponse>;

