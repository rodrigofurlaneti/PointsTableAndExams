using Microsoft.EntityFrameworkCore;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using PointsTableAndExams.Infrastructure.Data;

namespace PointsTableAndExams.Infrastructure.Data.Repositories;

public sealed class DailyLogRepository(AppDbContext context)
    : BaseRepository<DailyLog>(context), IDailyLogRepository
{
    /// <summary>
    /// Carrega COM tracking e inclui Items para que AddItem + SaveChanges funcione.
    /// </summary>
    public override async Task<DailyLog?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await DbSet
            .Include(d => d.Items)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

    /// <summary>
    /// Registra explicitamente os novos DailyLogItems no contexto.
    /// EF não detecta automaticamente adições a List&lt;T&gt; com backing field.
    /// </summary>
    public override Task UpdateAsync(DailyLog entity, CancellationToken ct = default)
    {
        foreach (var item in entity.Items)
        {
            if (Context.Entry(item).State == EntityState.Detached)
                Context.Set<DailyLogItem>().Add(item);
        }
        return Task.CompletedTask;
    }

    public async Task<DailyLog?> GetByUserAndDateAsync(Guid userId, DateOnly date, CancellationToken ct = default) =>
        await DbSet.AsNoTracking()
            .Include(d => d.Items).ThenInclude(i => i.FoodItem)
            .AsSplitQuery()
            .FirstOrDefaultAsync(d => d.UserId == userId && d.LogDate == date, ct);

    public async Task<IReadOnlyList<DailyLog>> GetHistoryAsync(Guid userId, DateOnly from, DateOnly to, CancellationToken ct = default) =>
        await DbSet.AsNoTracking()
            .Where(d => d.UserId == userId && d.LogDate >= from && d.LogDate <= to)
            .OrderByDescending(d => d.LogDate)
            .ToListAsync(ct);

    public async Task<bool> ExistsForDateAsync(Guid userId, DateOnly date, CancellationToken ct = default) =>
        await DbSet.AnyAsync(d => d.UserId == userId && d.LogDate == date, ct);

    /// <summary>
    /// Adiciona o item diretamente no DbSet e atualiza TotalPoints com ExecuteUpdateAsync
    /// (raw SQL UPDATE — sem change tracking, sem concurrency issues).
    /// </summary>
    public async Task<Guid> AddItemDirectAsync(Guid dailyLogId, DailyLogItem item, CancellationToken ct = default)
    {
        await Context.Set<DailyLogItem>().AddAsync(item, ct);
        await Context.SaveChangesAsync(ct);

        // Recalcula TotalPoints + atualiza DailyLog via SQL direto
        // (evita o bug de cast smallint→int do EF e o DbUpdateConcurrencyException)
        await Context.Database.ExecuteSqlRawAsync("""
            UPDATE DailyLog
            SET TotalPoints = (
                SELECT ISNULL(SUM(CAST(PointsComputed AS INT)), 0)
                FROM DailyLogItem
                WHERE DailyLogId = {0}
            ),
            UpdatedAt = {1}
            WHERE Id = {0}
            """, dailyLogId, DateTime.Now);

        return item.Id;
    }
}
