using System;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Hangfire;
using Infrastructure.Data;
using Amazon.S3;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.PostgreSql;
using Infrastructure.Repositories.Cold;
using Domain.Interfaces.Services;
using Infrastructure.Services;

namespace Infrastructure.DI;

public static class ServicesExtensions
{
    public static void ConfigureInfraServices(this IServiceCollection services)
    {
        services.AddScoped<IDbCommit, DbCommit>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<ILectureRepository, LectureRepository>();
        services.AddScoped<IDbCommit, DbCommit>();
        services.AddScoped<IMessageQueueService, RabbitMqService>();
        services.AddScoped<ITemporaryStorageService, TemporaryStorageService>();
        services.AddScoped<IAwsService, AwsService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
        services.AddScoped<IStudentLectureRepository, StudentLectureRepository>();
        services.AddScoped<IARepository, IARepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IParameterRepository, ParameterRepository>();
        services.AddScoped<IIARepository, IARepository>();
        services.AddScoped<IPictureRepository, PictureRepository>();
        services.AddScoped<ICourseProgressRepository, CourseProgressRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<IStudentLectureRepository, StudentLectureRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddAWSService<IAmazonS3>();
        services.AddHangfire(config => config.UsePostgreSqlStorage(
            StringConnection.BuildConnectionString() ?? string.Empty));
        services.AddHangfireServer();
    }

    public static void AddDataContexts(this IServiceCollection services)
    {
        services
            .AddDbContext<DomnumDbContext>(
                x => { x.UseNpgsql(StringConnection.BuildConnectionString()); });
    }
}
