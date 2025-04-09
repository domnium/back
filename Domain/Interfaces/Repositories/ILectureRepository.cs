using System;
using Domain.Entities.Core;

namespace Domain.Interfaces.Repositories;

public interface ILectureRepository : IBaseRepository<Lecture>
{
    Task<List<Lecture>> GetByModule(Guid moduleId, CancellationToken cancellationToken);
    Task MarkLectureAsCompleted(Guid courseid, Guid studentId, Guid lectureId, CancellationToken cancellationToken);
    Task<bool> IsLectureCompleted(Guid studentId, Guid lectureId, CancellationToken cancellationToken);
}
