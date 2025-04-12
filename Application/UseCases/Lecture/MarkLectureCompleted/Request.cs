using Domain.Records;
using MediatR;

namespace Application.UseCases.Lecture.MarkLectureCompleted;

public record Request(
    Guid CourseId,
    Guid StudentId,
    Guid LectureId
) : IRequest<BaseResponse>;
