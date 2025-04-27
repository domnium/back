using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetLastStudentCourse;

public record Request(Guid StudentId) : IRequest<BaseResponse<Response>>;