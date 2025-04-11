using Domain.Records;
using MediatR;

namespace Application.UseCases.CourseProgress.Get.StudentCourseProgress;

public record Request(Guid StudentId, Guid CourseId): IRequest<BaseResponse>;