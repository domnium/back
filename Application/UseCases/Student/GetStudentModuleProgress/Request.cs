using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetStudentModuleProgress;

public record Request(Guid StudentId, Guid ModuleId) 
: IRequest<BaseResponse<Response>>;