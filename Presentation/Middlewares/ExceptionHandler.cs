using System;

namespace Presentation.Middlewares;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandler> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro inesperado");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = new
            {
                message = "Ocorreu um erro interno no servidor. Tente novamente mais tarde.",
                devMessage = _env.IsDevelopment() ? ex : null
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
