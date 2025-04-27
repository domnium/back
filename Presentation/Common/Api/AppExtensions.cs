using System;
using Domain.Interfaces.Services;
using Hangfire;

namespace Presentation.Common.Api;

public static class AppExtensions
{
    #region ConfigureEnvironment
    public static void ConfigureDevEnvironment(this WebApplication app)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
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