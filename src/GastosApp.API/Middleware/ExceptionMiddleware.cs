using System.Net;
using System.Text.Json;

namespace GastosApp.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado procesando {Path}", context.Request.Path);

            var (status, mensaje) = ex switch
            {
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, ex.Message),
                InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var respuesta = JsonSerializer.Serialize(new { error = mensaje });
            await context.Response.WriteAsync(respuesta);
        }
    }
}
