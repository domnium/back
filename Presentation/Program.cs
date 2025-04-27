
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
    options.ListenAnyIP(5070, o => o.Protocols = HttpProtocols.Http1); 
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

var job = app.Services.GetRequiredService<IGeneratePresignedUrlJob>();
RecurringJob.AddOrUpdate<IGeneratePresignedUrlJob>(
    job => job.GenerateAndSavePresignedUrlsAsync(),
    Cron.Minutely); 

app.UseRouting();
app.UseMiddleware<ExceptionHandler>();
app.UseCors(Configuration.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();