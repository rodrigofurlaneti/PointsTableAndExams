using Microsoft.EntityFrameworkCore;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using PointsTableAndExams.Infrastructure.Data;

namespace PointsTableAndExams.Infrastructure.Data.Repositories;

public sealed class DailyLogRepository(AppDbContext context)
    : BaseRepository<DailyLog>(context), IDailyLogRepository
{
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
}
