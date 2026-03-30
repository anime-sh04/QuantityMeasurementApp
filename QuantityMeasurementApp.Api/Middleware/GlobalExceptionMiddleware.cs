using System.Net;
using System.Text.Json;

namespace QuantityMeasurementApp.Api.Middleware
{
    /// <summary>
    /// Catches unhandled exceptions and returns a consistent JSON error envelope.
    /// Prevents stack traces from leaking to the client in production.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next,
                                         ILogger<GlobalExceptionMiddleware> logger,
                                         IWebHostEnvironment env)
        {
            _next   = next;
            _logger = logger;
            _env    = env;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception on {Method} {Path}",
                    ctx.Request.Method, ctx.Request.Path);

                ctx.Response.StatusCode  = (int)HttpStatusCode.InternalServerError;
                ctx.Response.ContentType = "application/json";

                var payload = new
                {
                    message = "An unexpected error occurred.",
                    detail  = _env.IsDevelopment() ? ex.Message : null
                };

                await ctx.Response.WriteAsync(
                    JsonSerializer.Serialize(payload,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            }
        }
    }
}
