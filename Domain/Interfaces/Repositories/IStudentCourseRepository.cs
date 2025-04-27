using System;
using System.Linq.Expressions;
using Domain.Entities.Relationships;

namespace Domain.Interfaces.Repositories;

public interface IStudentCourseRepository : IBaseRepository<StudentCourse>
{
    Task<CoursePopularDto?> GetLastStudentCourseAsync(
        Expression<Func<StudentCourse, bool>> predicate,
        CancellationToken cancellationToken = default
    );
}
