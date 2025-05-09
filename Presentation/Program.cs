
using Domain;
using Domain.Interfaces.Services;
using Hangfire;
using Infrastructure.DI;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Presentation.Common.Api;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
   options.ListenAnyIP(7069, listenOptions =>
    {
        listenOptions.UseHttps(); // Usa o certificado padrão
    });
    options.ListenAnyIP(5070); // Porta HTTP
});

builder.AddConfiguration();
builder.AddSecurity();
builder.AddCrossOrigin();
builder.Services.AddDataContexts();
builder.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ConfigureDevEnvironment();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseForwardedHeaders();
app.UseHangfireServer();

var lifetime = app.Lifetime;

lifetime.ApplicationStarted.Register(() =>
{
    using var scope = app.Services.CreateScope();
    var job = scope.ServiceProvider.GetRequiredService<IGeneratePresignedUrlJob>();
    Task.Run(async () =>
    {
        await Task.Delay(2000);
        RecurringJob.AddOrUpdate<IGeneratePresignedUrlJob>(
            job => job.GenerateAndSavePresignedUrlsAsync(),
            Cron.Minutely);
    });
});

app.UseRouting();
app.UseMiddleware<ExceptionHandler>();
app.UseCors(Configuration.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();