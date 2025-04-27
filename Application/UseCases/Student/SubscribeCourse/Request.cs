using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.SubscribeCourse;

/// <summary>
/// Representa a requisição para inscrever um estudante em um curso.
/// </summary>
public record Request(
    Guid StudentId,
    Guid CourseId
) : IRequest<BaseResponse<object>>;