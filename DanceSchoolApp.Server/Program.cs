using System;
using System.Text;
using System.Reflection;
using DanceSchoolApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
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


// ── Controllers ───────────────────────────────────────────────────────────
builder.Services.AddControllers();


// ── OpenAPI ───────────────────────────────────────────────────────────────
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();



// ── Database ──────────────────────────────────────────────────────────────
var conn = Environment.GetEnvironmentVariable("DanceSchoolApp_DB");
if (conn == null) Console.WriteLine("Warning - Failed to get Db enviromental variable !");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(conn));

// ── JWT Authentication ─────────────────────────────────────────────────────
var jwtSecret = Environment.GetEnvironmentVariable("DanceSchoolApp_JWT_Secret");

if (string.IsNullOrWhiteSpace(jwtSecret))
    Console.WriteLine("Warning — JWT secret environment variable is not set.");

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

            // Map JWT role claims to ASP.NET Core's ClaimTypes.Role so
            // [Authorize(Roles = "staff")] works correctly out of the box.
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

// ─────────────────────────────────────────────────────────────────────────

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
