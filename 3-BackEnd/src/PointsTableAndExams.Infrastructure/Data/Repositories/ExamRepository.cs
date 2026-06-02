using Microsoft.EntityFrameworkCore;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using PointsTableAndExams.Infrastructure.Data;

namespace PointsTableAndExams.Infrastructure.Data.Repositories;

public sealed class ExamRepository(AppDbContext context)
    : BaseRepository<Exam>(context), IExamRepository
{
    public async Task<IReadOnlyList<Exam>> GetByCategoryAsync(
        Guid categoryId, CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Include(e => e.Category)
            .Where(e => e.ExamCategoryId == categoryId && e.IsActive)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Exam>> SearchByNameAsync(
        string name, CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Include(e => e.Category)
            .Where(e => e.IsActive && (string.IsNullOrEmpty(name) || e.Name.Contains(name)))
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);

    public override async Task<IReadOnlyList<Exam>> GetAllAsync(
        CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Include(e => e.Category)
            .Where(e => e.IsActive)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);
}
