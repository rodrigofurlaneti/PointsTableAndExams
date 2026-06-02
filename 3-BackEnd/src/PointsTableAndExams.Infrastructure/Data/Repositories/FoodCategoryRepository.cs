using Microsoft.EntityFrameworkCore;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using PointsTableAndExams.Infrastructure.Data;

namespace PointsTableAndExams.Infrastructure.Data.Repositories;

public sealed class FoodCategoryRepository(AppDbContext context)
    : BaseRepository<FoodCategory>(context), IFoodCategoryRepository
{
    public async Task<IReadOnlyList<FoodCategory>> GetActiveWithItemsAsync(
        CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Include(c => c.Items)
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<FoodCategory>> SearchByNameAsync(
        string name, CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Where(c => c.IsActive && (string.IsNullOrEmpty(name) || c.Name.Contains(name)))
            .OrderBy(c => c.SortOrder)
            .ToListAsync(cancellationToken);

    public override async Task<IReadOnlyList<FoodCategory>> GetAllAsync(
        CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ToListAsync(cancellationToken);
}
