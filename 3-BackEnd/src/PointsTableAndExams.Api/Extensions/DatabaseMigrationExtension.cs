using Microsoft.EntityFrameworkCore;
using PointsTableAndExams.Infrastructure.Data;

namespace PointsTableAndExams.Api.Extensions;

/// <summary>
/// Roda as migrations do EF Core automaticamente no startup.
/// Útil em produção (Azure App Service) para manter o banco atualizado
/// sem precisar rodar dotnet-ef manualmente.
/// </summary>
public static class DatabaseMigrationExtension
{
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Checking pending EF migrations…");
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

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
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
            // Re-throw para impedir que a aplicação suba com banco desatualizado
            throw;
        }
    }
}
