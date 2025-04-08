using System;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories;


namespace Infrastructure.Repositories
{
    public class DbCommit(HotDbContext context) : IDbCommit
    {
        public async Task Commit(CancellationToken cancellationToken)
            => await context.SaveChangesAsync(cancellationToken);
    }
}