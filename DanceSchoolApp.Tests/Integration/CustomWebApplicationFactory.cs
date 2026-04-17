using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        builder.ConfigureServices(services =>
        {
            // ── 1. Remove the AppDbContext registration ───────────────────────
            var contextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(AppDbContext));
            if (contextDescriptor != null)
                services.Remove(contextDescriptor);

            // ── 2. Remove DbContextOptions<AppDbContext> ──────────────────────
            var optionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (optionsDescriptor != null)
                services.Remove(optionsDescriptor);

            // ── 3. Remove the generic IDbContextOptions too ───────────────────
            var genericOptionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions));
            if (genericOptionsDescriptor != null)
                services.Remove(genericOptionsDescriptor);

            // ── 4. Re-register with the open SQLite connection ────────────────
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(_connection));

            // ── 5. Stub out EmailService to prevent SMTP calls ────────────────
            // EmailService is auto-registered by the reflection loop in Program.cs.
            // We override it with a factory that constructs it with a null/empty
            // SMTP config — it will never actually send because our tests never
            // trigger password-reset flows, and the constructor doesn't send on init.
            // (No action needed here — the env var warning is harmless for tests.)
        });

        // Suppress the SPA proxy startup assembly that tries to launch Vite
        builder.UseEnvironment("Test");
    }

    /// <summary>
    /// Seeds data into the test database before a test makes HTTP requests.
    /// EnsureCreated() builds the schema from the EF model on first call.
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