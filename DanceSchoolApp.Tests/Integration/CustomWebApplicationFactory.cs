using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using DanceSchoolApp.Server.Data;
using DanceSchoolApp.Server.Services;

namespace DanceSchoolApp.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string TestJwtSecret = "SuperSecretTestKey_AtLeast32Characters!!";

    private readonly SqliteConnection _connection;

    public CustomWebApplicationFactory()
    {
        // Set before the host builds so Program.cs startup warnings are suppressed
        // and the JWT closure captures the test secret.
        Environment.SetEnvironmentVariable("DanceSchoolApp_DB", "test-placeholder");
        Environment.SetEnvironmentVariable("DanceSchoolApp_JWT_Secret", TestJwtSecret);

        // Keep a single open connection for the lifetime of the factory so that
        // EF Core's in-memory SQLite database is not destroyed between requests.
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // ── Replace AppDbContext with SQLite in-memory ────────────────────
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            var contextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(AppDbContext));
            if (contextDescriptor != null) services.Remove(contextDescriptor);

            // Pass the existing open connection so EF does not close it between
            // scopes, which would wipe the in-memory database.
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(_connection));

            // ── Replace EmailService with a no-op stub ────────────────────────
            var emailDescriptors = services
                .Where(d => d.ServiceType == typeof(EmailService))
                .ToList();
            foreach (var d in emailDescriptors)
                services.Remove(d);

            services.AddScoped<EmailService, NoOpEmailService>();

            // ── Override JWT signing key ──────────────────────────────────────
            // Program.cs reads the env var into a captured local variable, so we
            // PostConfigure here as a belt-and-suspenders guarantee that the test
            // key is always used regardless of capture timing.
            services.PostConfigure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters.IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(TestJwtSecret));
                });
        });
    }

    /// <summary>
    /// Seeds the test database inside a dedicated scope. Call this before
    /// making HTTP requests that depend on pre-existing data.
    /// </summary>
    public void SeedDatabase(Action<AppDbContext> seeder)
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        seeder(db);
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

    // ── No-op email stub ─────────────────────────────────────────────────────
    // EmailService.SendWelcomeEmailAsync and SendPasswordResetEmailAsync are
    // declared virtual so this subclass can safely silence them in tests.
    private sealed class NoOpEmailService : EmailService
    {
        public NoOpEmailService(IConfiguration config, ILogger<EmailService> logger)
            : base(config, logger) { }

        public override Task SendWelcomeEmailAsync(
            string toEmail, string role, string setupLink) => Task.CompletedTask;

        public override Task SendPasswordResetEmailAsync(
            string toEmail, string resetLink) => Task.CompletedTask;
    }
}
