using System;
using Domain.Entities.Core;

namespace Domain.Interfaces.Repositories;

 public interface ITeacherRepository : IBaseRepository<Teacher>
{
    public Task<List<Teacher>> GetByCourse(Guid courseId, CancellationToken cancellationToken);
    public Task<Teacher?> GetByCpf(string cpf, CancellationToken cancellationToken);
    public Task<List<Teacher>> GetAllByIds(IEnumerable<Guid?>? ids, CancellationToken cancellationToken);
    public Task<Teacher?> GetByEmail(string email, CancellationToken cancellationToken);
}
