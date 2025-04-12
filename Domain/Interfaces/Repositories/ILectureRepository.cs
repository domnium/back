using System;
using Domain.Entities.Core;

namespace Domain.Interfaces.Repositories;

public interface ILectureRepository : IBaseRepository<Lecture>
{
    Task<bool?> IsLectureCompleted(Guid studentId, Guid lectureId, CancellationToken cancellationToken);
}
