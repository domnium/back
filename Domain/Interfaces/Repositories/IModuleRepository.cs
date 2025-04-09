using System;
using Domain.Entities.Core;

namespace Domain.Interfaces.Repositories;

public interface IModuleRepository : IBaseRepository<Module>
{
    Task<List<Module>> GetByCourse(Guid courseId, CancellationToken cancellationToken);
}