using System;
using Application.Services;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Hangfire;
using Infrastructure.Data;
using Amazon.S3;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.PostgreSql;

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
        services.AddScoped<IAwsService, AwsService>();
        services.AddScoped<IPictureRepository, PictureRepository>();
        services.AddScoped<ICourseProgressRepository, CourseProgressRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<IStudentLectureRepository, StudentLectureRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddAWSService<IAmazonS3>();
        services.AddHangfire(config => config.UsePostgreSqlStorage(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")));
        services.AddHangfireServer();
    }

    public static void AddDataContexts(this IServiceCollection services)
    {
        services
            .AddDbContext<DomnumDbContext>(
                x => { x.UseNpgsql(StringConnection.BuildConnectionString()); });
    }
}
