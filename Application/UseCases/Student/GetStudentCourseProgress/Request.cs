using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetStudentCourseProgress;

public record Request(Guid StudentId, Guid CourseId) 
: IRequest<BaseResponse<Response>>;