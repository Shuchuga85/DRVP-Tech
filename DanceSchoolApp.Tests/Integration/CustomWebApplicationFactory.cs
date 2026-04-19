using DanceSchoolApp.Server.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DanceSchoolApp.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection;

    public CustomWebApplicationFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        Environment.SetEnvironmentVariable("DanceSchoolApp_JWT_Secret",
            "SuperSecretTestKey_AtLeast32Characters!!");
        Environment.SetEnvironmentVariable("DanceSchoolApp_DB",
            "placeholder-suppresses-warning");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            // Remove EVERY descriptor whose ServiceType name contains
            // "DbContext" or "DbContextOptions" — this catches all the
            // EF Core internal registrations (options, options-configuration,
            // the context itself) regardless of EF Core version.
            var toRemove = services
                .Where(d =>
                    d.ServiceType == typeof(AppDbContext) ||
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    (d.ServiceType.IsGenericType &&
                     d.ServiceType.GetGenericTypeDefinition().FullName != null &&
                     d.ServiceType.GetGenericTypeDefinition().FullName!
                         .Contains("IDbContextOptionsConfiguration")))
                .ToList();

            foreach (var d in toRemove)
                services.Remove(d);

            // Re-register AppDbContext against the shared in-memory SQLite
            // connection. The connection must stay open for the entire lifetime
            // of the factory or the in-memory DB is destroyed between scopes.
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(_connection));
        });
    }

    /// <summary>
    /// Runs a seeder action inside a dedicated DI scope so tests can populate
    /// the in-memory database before sending HTTP requests.
    /// EnsureCreated() is idempotent — safe to call from multiple tests.
    /// </summary>
    public void SeedDatabase(Action<AppDbContext> seeder)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        seeder(db);
        db.SaveChanges();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}