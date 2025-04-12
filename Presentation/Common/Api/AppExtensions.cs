using System;
using Domain.Interfaces.Services;
using Hangfire;

namespace Presentation.Common.Api;

public static class AppExtensions
{
    #region ConfigureEnvironment
    public static void ConfigureEnvironment(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseForwardedHeaders();
        app.UseHangfireServer();

        var job = app.Services.GetRequiredService<IGeneratePresignedUrlJob>();
        RecurringJob.AddOrUpdate<IGeneratePresignedUrlJob>(
            job => job.GenerateAndSavePresignedUrlsAsync(),
            Cron.Minutely); 
    }
    #endregion ConfigureEnvironment

    #region Security
    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
    #endregion
}