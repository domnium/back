using System;
using Domain.Entities;
using Domain.Entities.Core;

namespace Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> Authenticate(User user, CancellationToken cancellationToken);
    Task<User> ActivateUserAsync(string email, Guid token, CancellationToken cancellationToken);
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
}