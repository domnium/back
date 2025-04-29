using System;
using Domain.Entities;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Cold;

public class UserRepository(DomnumDbContext context) 
    : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> Authenticate(User user, CancellationToken cancellationToken)
    {
        var userFromDb = await context.Set<User>().AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Address == user.Email.Address && u.Active, cancellationToken);
        return userFromDb;
    }

    public async Task<User?> ActivateUserAsync(string email, long token, CancellationToken cancellationToken)
    {
       var user =  (await context.Set<User>().AsNoTracking()
            .FirstOrDefaultAsync(x => !x.Active && x.Email.Address!.Equals(email) && x.TokenActivate.Equals(token),
                cancellationToken: cancellationToken));
        if(user is not null){
            user?.AssignActivate(true);
            Update(user);
        }
       return user;
    }

    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken) =>
        await context.Set<User>().AsNoTracking().FirstOrDefaultAsync(x => x.Email.Address.Equals(email), cancellationToken);
}
