using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Domain.Interfaces.Repositories;

public interface IFoodItemRepository : IRepository<FoodItem>
{
    Task<IReadOnlyList<FoodItem>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FoodItem>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
}
