
using Domain;
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

app.ConfigureEnvironment();

app.UseRouting();
app.UseMiddleware<ExceptionHandler>();
app.UseCors(Configuration.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();