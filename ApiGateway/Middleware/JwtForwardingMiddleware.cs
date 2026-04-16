using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGateway.Middleware
{
    /// <summary>
    /// Validates the Bearer JWT on inbound requests and, if valid, adds
    /// X-User-Id / X-User-Role headers so downstream microservices can trust
    /// the caller identity without re-validating the token themselves.
    /// </summary>
    public class JwtForwardingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration  _config;
        private readonly ILogger<JwtForwardingMiddleware> _logger;

        public JwtForwardingMiddleware(
            RequestDelegate next,
            IConfiguration  config,
            ILogger<JwtForwardingMiddleware> logger)
        {
            _next   = next;
            _config = config;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var auth = context.Request.Headers.Authorization.FirstOrDefault();
            if (auth is not null && auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = auth["Bearer ".Length..].Trim();
                var principal = TryValidate(token);
                if (principal is not null)
                {
                    var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? principal.FindFirst("sub")?.Value;
                    var role   = principal.FindFirst(ClaimTypes.Role)?.Value;

                    if (userId is not null)
                        context.Request.Headers["X-User-Id"]   = userId;
                    if (role is not null)
                        context.Request.Headers["X-User-Role"] = role;

                    _logger.LogDebug("Forwarding userId={UserId} role={Role}", userId, role);
                }
            }

            await _next(context);
        }

        private ClaimsPrincipal? TryValidate(string token)
        {
            try
            {
                var jwtSection = _config.GetSection("Jwt");
                var handler    = new JwtSecurityTokenHandler();
                var key        = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));

                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = jwtSection["Issuer"],
                    ValidAudience            = jwtSection["Audience"],
                    IssuerSigningKey         = key,
                    ClockSkew                = TimeSpan.Zero
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
