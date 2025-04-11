using Domain.Records;
using MediatR;

namespace Application.UseCases.CourseProgress.Get.StudentModuleProgress;

public record Request(Guid StudentId, Guid ModuleId) 
: IRequest<BaseResponse>;
