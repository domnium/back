using System;
using Domain.Entities.Core;

namespace Domain.Interfaces.Repositories;

 public interface ICourseProgressRepository
{
    Task<decimal> GetModuleProgress(Guid studentId, Guid moduleId);
    Task<decimal> GetCourseProgress(Guid studentId, Guid courseId);
    Task<IList<Guid>> GetLecturesCompleted(Guid courseid, Guid studentId, CancellationToken cancellationToken);
    public Task<Course> GetMostPopularCourseAsync();
}