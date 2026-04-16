using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ConversionService.Data;
using ConversionService.Middleware;
using ConversionService.Repository;
using ConversionService.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── EF Core ────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<ConversionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── DI ─────────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IConversionRepository, ConversionRepository>();
builder.Services.AddScoped<IConversionService,    ConversionServiceImpl>();

// ── JWT (validate tokens issued by AuthService) ────────────────────────────
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
        options.Events = new JwtBearerEvents
        {
            OnChallenge = ctx =>
            {
                ctx.HandleResponse();
                ctx.Response.StatusCode  = 401;
                ctx.Response.ContentType = "application/json";
                return ctx.Response.WriteAsync("{\"message\":\"Unauthorized. Please provide a valid Bearer token.\"}");
            }
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
        Title   = "Conversion Service API",
        Version = "v1",
        Description = "Microservice for unit conversion operations: compare, add, subtract, divide, convert.\n\n" +
                      "**Supported units:**\n" +
                      "- Length: `Feet` `Inches` `Yards` `Centimeters`\n" +
                      "- Weight: `Kilogram` `Gram` `Pound`\n" +
                      "- Volume: `Litre` `Millilitre` `Gallon`\n" +
                      "- Temperature: `Celsius` `Fahrenheit` `Kelvin`\n\n" +
                      "**Port:** 5002"
    });
    var secScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization", Type = SecuritySchemeType.Http,
        Scheme = "bearer", BearerFormat = "JWT", In = ParameterLocation.Header
    };
    c.AddSecurityDefinition("Bearer", secScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, [] }
    });
});

var app = builder.Build();

// Ensure QuantityMeasurements table exists (schema shared with HistoryService)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ConversionDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
