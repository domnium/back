using System;
using System.Reflection;
using Application.Messaging.Consumers;
using Domain;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI;
public static class ServicesExtensions
{
    public static void AppServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }

    public static void AddRabbitMQ(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UploadFileConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
               cfg.Host(Configuration.RabbitMQHost, h =>
                {
                    h.Username(Configuration.RabbitMQUser);
                    h.Password(Configuration.RabbitMQPassword);
                });

                cfg.ReceiveEndpoint("upload_queue", e =>
                {
                    e.ConfigureConsumer<UploadFileConsumer>(ctx);
                });
            });
        });
    }
}