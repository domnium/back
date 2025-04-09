using System;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class ServicesExtensions
{
    public static void ConfigureInfraServices(this IServiceCollection services)
    {
        services.AddScoped<IDbCommit, DbCommit>();
    }

    public static void AddDataContexts(this IServiceCollection services)
    {
        services
            .AddDbContext<DomnumDbContext>(
                x => { x.UseNpgsql(StringConnection.BuildConnectionString()); });
    }
}
