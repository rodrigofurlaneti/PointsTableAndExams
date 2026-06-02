using Microsoft.EntityFrameworkCore;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using PointsTableAndExams.Infrastructure.Data;

namespace PointsTableAndExams.Infrastructure.Data.Repositories;

public sealed class ExamCategoryRepository(AppDbContext context)
    : BaseRepository<ExamCategory>(context), IExamCategoryRepository
{
    public async Task<IReadOnlyList<ExamCategory>> GetActiveWithExamsAsync(
        CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Include(c => c.Exams)
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ToListAsync(cancellationToken);

    public override async Task<IReadOnlyList<ExamCategory>> GetAllAsync(
        CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ToListAsync(cancellationToken);
}
