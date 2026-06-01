using Microsoft.EntityFrameworkCore;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using PointsTableAndExams.Infrastructure.Data;

namespace PointsTableAndExams.Infrastructure.Data.Repositories;

public sealed class FoodItemRepository(AppDbContext context)
    : BaseRepository<FoodItem>(context), IFoodItemRepository
{
    public async Task<IReadOnlyList<FoodItem>> GetByCategoryAsync(
        Guid categoryId, CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Include(f => f.Category)
            .Where(f => f.FoodCategoryId == categoryId && f.IsActive)
            .OrderBy(f => f.Name)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<FoodItem>> SearchByNameAsync(
        string name, CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Include(f => f.Category)
            .Where(f => f.IsActive && (string.IsNullOrEmpty(name) || f.Name.Contains(name)))
            .OrderBy(f => f.Category != null ? f.Category.SortOrder : 0)
            .ThenBy(f => f.Name)
            .ToListAsync(cancellationToken);

    public override async Task<IReadOnlyList<FoodItem>> GetAllAsync(
        CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Include(f => f.Category)
            .Where(f => f.IsActive)
            .OrderBy(f => f.Category != null ? f.Category.SortOrder : 0)
            .ThenBy(f => f.Name)
            .ToListAsync(cancellationToken);
}
