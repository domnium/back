using System;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;


namespace Infrastructure.Repositories
{
    public class DbCommit(DomnumDbContext context) : IDbCommit
    {
        public async Task Commit(CancellationToken cancellationToken)
            => await context.SaveChangesAsync(cancellationToken);
    }
}