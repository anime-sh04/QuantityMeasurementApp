using ApiGateway.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── YARP Reverse Proxy ─────────────────────────────────────────────────────
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ── JWT (used only for the /health and optional gateway-level endpoints) ───
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey     = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSection["Issuer"],
            ValidAudience            = jwtSection["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(jwtKey),
            ClockSkew                = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(o => o.AddPolicy("AllowAll",
    p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "API Gateway",
        Version     = "v1",
        Description = "Entry point for all client requests. Routes to Auth (5001), Conversion (5002) and History (5003) services.\n\n" +
                      "| Prefix | Downstream Service |\n" +
                      "|---|---|\n" +
                      "| `/api/auth/**` | Auth Service :5001 |\n" +
                      "| `/api/quantity/compare,add,subtract,divide,convert` | Conversion Service :5002 |\n" +
                      "| `/api/quantity/history/**`, `/api/quantity/stats` | History Service :5003 |\n\n" +
                      "**Port:** 5000"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", Type = SecuritySchemeType.Http,
        Scheme = "bearer", BearerFormat = "JWT", In = ParameterLocation.Header,
        Description = "Paste your JWT here. Obtained from POST /api/auth/login"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, [] }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Inject X-User-Id / X-User-Role headers into proxied requests
app.UseMiddleware<JwtForwardingMiddleware>();

// Health-check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    status    = "Healthy",
    timestamp = DateTime.UtcNow,
    services  = new[]
    {
        new { name = "AuthService",       port = 5001 },
        new { name = "ConversionService", port = 5002 },
        new { name = "HistoryService",    port = 5003 }
    }
}));

app.MapControllers();
app.MapReverseProxy();
app.Run();
