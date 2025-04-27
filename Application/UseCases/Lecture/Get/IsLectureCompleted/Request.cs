using Domain.Records;
using MediatR;

namespace Application.UseCases.Lecture.Get.IsLectureCompleted;

public record Request(
    Guid StudentId,
    Guid LectureId
) : IRequest<BaseResponse<Response>>;