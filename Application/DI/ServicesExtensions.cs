using System;
using System.Reflection;
using Application.Jobs;
using Application.Messaging.Consumers;
using Domain;
using Domain.Interfaces.Services;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI;
public static class ServicesExtensions
{
    public static void AppServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddSingleton<IGeneratePresignedUrlJob, GeneratePresignedUrl>();
    }

    public static void AddRabbitMQ(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UploadFileConsumer>();
            x.AddConsumer<DeleteFileConsumer>(); 
            x.AddConsumer<EmailConsumer>(); 

            x.UsingRabbitMq((ctx, cfg) =>
            {
               cfg.Host(Configuration.RabbitMQHost,5672, "/", h =>
                {
                    h.Username(Configuration.RabbitMQUser);
                    h.Password(Configuration.RabbitMQPassword);
                });

                cfg.ReceiveEndpoint("upload_queue", e =>
                {
                    e.ConfigureConsumer<UploadFileConsumer>(ctx);
                });
                
                cfg.ReceiveEndpoint("emails_queue", e =>
                {
                    e.ConfigureConsumer<EmailConsumer>(ctx);
                });
                
                cfg.ReceiveEndpoint("delete_queue", e =>
                {
                    e.ConfigureConsumer<DeleteFileConsumer>(ctx);
                });
            });
        });
    }
}