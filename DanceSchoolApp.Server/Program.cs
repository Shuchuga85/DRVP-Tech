using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using DanceSchoolApp.Server.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var conn = Environment.GetEnvironmentVariable("DanceSchoolApp_DB");
if (conn == null) Console.WriteLine("Warning - Failed to get Db enviromental variable !");


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("DanceSchoolApp_DB")));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
