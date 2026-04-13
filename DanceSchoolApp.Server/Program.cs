using System;
using System.Text;
using System.Reflection;
using DanceSchoolApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ── Auto-register all services via reflection ──────────────────────────────
var serviceTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsClass && t.Name.EndsWith("Service"));

foreach (var service in serviceTypes)
{
    builder.Services.AddScoped(service);
}

// ── Controllers ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── OpenAPI ─────────────────────────────────────────────────────────────────
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// ── Database ────────────────────────────────────────────────────────────────
var conn = Environment.GetEnvironmentVariable("DanceSchoolApp_DB");
if (string.IsNullOrWhiteSpace(conn))
{
    Console.WriteLine("Warning - Failed to get Db environmental variable!");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(conn));

// ── JWT Authentication ──────────────────────────────────────────────────────
var jwtSecret = Environment.GetEnvironmentVariable("DanceSchoolApp_JWT_Secret");

if (string.IsNullOrWhiteSpace(jwtSecret))
{
    throw new InvalidOperationException(
        "A variável de ambiente 'DanceSchoolApp_JWT_Secret' não está definida."
    );
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "DanceSchoolApp",
            ValidAudience = "DanceSchoolApp",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret)),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

// ── Authorization ───────────────────────────────────────────────────────────
builder.Services.AddAuthorization();

// ── CORS ────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "https://localhost:5173",
                "http://localhost:5173",
                "https://localhost:5174",
                "http://localhost:5174"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// ── Pipeline ────────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.MapStaticAssets();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();