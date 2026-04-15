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

// ── Controllers ────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── OpenAPI ────────────────────────────────────────────────────────────────
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// ── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("https://localhost:5173", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ── Database ───────────────────────────────────────────────────────────────
var conn = Environment.GetEnvironmentVariable("DanceSchoolApp_DB");
if (string.IsNullOrWhiteSpace(conn))
    Console.WriteLine("Warning - Failed to get DB environment variable!");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(conn));

// ── JWT Authentication (cookie-based) ─────────────────────────────────────
var jwtSecret = Environment.GetEnvironmentVariable("DanceSchoolApp_JWT_Secret");
if (string.IsNullOrWhiteSpace(jwtSecret))
    Console.WriteLine("Warning - DanceSchoolApp_JWT_Secret environment variable is not set.");

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
                Encoding.UTF8.GetBytes(jwtSecret ?? string.Empty)),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["jwt"];
                if (!string.IsNullOrWhiteSpace(token))
                    context.Token = token;

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// ── Build app ──────────────────────────────────────────────────────────────
var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// CORS tem de vir antes de Authentication/Authorization
app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();