using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Domain.Interfaces.Repositories;

public interface IFoodCategoryRepository : IRepository<FoodCategory>
{
    Task<IReadOnlyList<FoodCategory>> GetActiveWithItemsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FoodCategory>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
}
