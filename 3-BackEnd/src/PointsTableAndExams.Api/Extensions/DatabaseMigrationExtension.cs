using Microsoft.EntityFrameworkCore;
using PointsTableAndExams.Infrastructure.Data;
using PointsTableAndExams.Infrastructure.Data.Seeds;

namespace PointsTableAndExams.Api.Extensions;

/// <summary>
/// Roda as migrations do EF Core e o seed de dados de referência
/// automaticamente no startup (Azure App Service / local dev).
/// </summary>
public static class DatabaseMigrationExtension
{
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // ── 1. Migrations ─────────────────────────────────────────────────
            logger.LogInformation("Checking pending EF migrations…");
            var pending = await db.Database.GetPendingMigrationsAsync();
            if (pending.Any())
            {
                logger.LogInformation("Applying {Count} migration(s): {Names}",
                    pending.Count(), string.Join(", ", pending));
                await db.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("Database is up to date. No migrations needed.");
            }

            // ── 2. Seed reference data ────────────────────────────────────────
            logger.LogInformation("Running database seeder…");
            await DatabaseSeeder.SeedAsync(db, logger);
            logger.LogInformation("Database seeder completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database migration/seeding.");
            throw;
        }
    }
}
