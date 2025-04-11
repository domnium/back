using System;
using Domain.Entities.Core;

namespace Domain.Interfaces.Repositories;
public interface ICourseRepository : IBaseRepository<Course>
{
    Task<int> GetStudentCountByCourseIdAsync(Guid courseId);
    Task<List<CoursePopularDto>> TopFiveMostPopular(CancellationToken cancellationToken);
}