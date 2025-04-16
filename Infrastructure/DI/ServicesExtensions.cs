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
        services.AddScoped<IDbCommit, DbCommit>()
        .AddScoped<ICourseRepository, CourseRepository>()
        .AddScoped<ITeacherRepository, TeacherRepository>()
        .AddScoped<ICategoryRepository, CategoryRepository>()
        .AddScoped<IModuleRepository, ModuleRepository>()
        .AddScoped<ILectureRepository, LectureRepository>()
        .AddScoped<IDbCommit, DbCommit>()
        .AddScoped<IMessageQueueService, RabbitMqService>()
        .AddScoped<ITemporaryStorageService, TemporaryStorageService>()
        .AddScoped<IAwsService, AwsService>()
        .AddScoped<IEmailService, EmailService>()
        .AddScoped<ITokenService, TokenService>()
        .AddScoped<IStudentCourseRepository, StudentCourseRepository>()
        .AddScoped<IStudentLectureRepository, StudentLectureRepository>()
        .AddScoped<IARepository, IARepository>()
        .AddScoped<IUserRepository, UserRepository>()
        .AddScoped<IParameterRepository, ParameterRepository>()
        .AddScoped<IIARepository, IARepository>()
        .AddScoped<IPictureRepository, PictureRepository>()
        .AddScoped<ICourseProgressRepository, CourseProgressRepository>()
        .AddScoped<IStudentRepository, StudentRepository>()
        .AddScoped<IVideoRepository, VideoRepository>()
        .AddScoped<IStudentLectureRepository, StudentLectureRepository>()
        .AddScoped<IJobRepository, JobRepository>()
        .AddAWSService<IAmazonS3>()
        .AddHangfire(config => config.UsePostgreSqlStorage(
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
