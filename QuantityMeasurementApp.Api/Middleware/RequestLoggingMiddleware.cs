namespace QuantityMeasurementApp.Api.Middleware
{
    /// <summary>
    /// Assigns a unique X-Correlation-Id header to every request and logs
    /// method, path, status code, and duration.  Strips sensitive headers
    /// (Authorization) from logs.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next,
                                        ILogger<RequestLoggingMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            var correlationId = ctx.Request.Headers["X-Correlation-Id"]
                                   .FirstOrDefault()
                                ?? Guid.NewGuid().ToString("N");

            ctx.Response.Headers["X-Correlation-Id"] = correlationId;

            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                await _next(ctx);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation(
                    "[{CorrelationId}] {Method} {Path} → {StatusCode} in {ElapsedMs}ms",
                    correlationId,
                    ctx.Request.Method,
                    ctx.Request.Path,
                    ctx.Response.StatusCode,
                    sw.ElapsedMilliseconds);
            }
        }
    }
}
