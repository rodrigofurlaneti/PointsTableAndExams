using Microsoft.EntityFrameworkCore;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using PointsTableAndExams.Infrastructure.Data;

namespace PointsTableAndExams.Infrastructure.Data.Repositories;

public sealed class ExamRequestRepository(AppDbContext context)
    : BaseRepository<ExamRequest>(context), IExamRequestRepository
{
    public async Task<IReadOnlyList<ExamRequest>> GetByUserAsync(Guid userId, CancellationToken ct = default) =>
        await DbSet.AsNoTracking()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.RequestDate)
            .ToListAsync(ct);

    public async Task<ExamRequest?> GetWithItemsAsync(Guid requestId, CancellationToken ct = default) =>
        await DbSet
            .Include(r => r.Items).ThenInclude(i => i.Exam)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == requestId, ct);

    public async Task<IReadOnlyList<ExamRequest>> GetPendingByUserAsync(Guid userId, CancellationToken ct = default) =>
        await DbSet.AsNoTracking()
            .Include(r => r.Items)
            .Where(r => r.UserId == userId && r.Items.Any(i => !i.IsCompleted))
            .AsSplitQuery()
            .ToListAsync(ct);
}
