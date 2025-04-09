using System;
using Domain.Entities;
using Domain.Entities.Core;
using Domain.Entities.Relationships;
using Domain.Interfaces.Repositories;
using Domain.Records;

namespace Domain.Interfaces;

public interface IStudentRepository : IBaseRepository<Student>
{
    Task<bool> IsStudentEnrolledInCourseAsync(Guid studentId, Guid courseId);
    Task<Guid> GetGuidAsync(long id, CancellationToken cancellationToken);
    Task EnrollStudentInCourseAsync(Guid studentId, Guid courseId);
    Task<BaseResponse> GetUserAsync(string emailStudent, CancellationToken cancellationToken);
    Task<bool> SaveIdArchive(Guid? studentId, Picture archive);
    Task<IList<StudentCourse>> GetEnrollStudentInCourseAsync(Guid studentId, CancellationToken cancellationToken);
}