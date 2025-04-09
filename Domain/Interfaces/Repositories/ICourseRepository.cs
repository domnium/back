using System;
using Domain.Entities.Core;

namespace Domain.Interfaces.Repositories;
public interface ICourseRepository : IBaseRepository<Course>
{
    Task<List<Course>> GetByCategory(Guid categoryId, CancellationToken cancellationToken);
    Task<List<Course>> SearchAsync(string searchQuery, CancellationToken cancellationToken);
    Task<int> GetStudentCountByCourseIdAsync(Guid courseId);
}