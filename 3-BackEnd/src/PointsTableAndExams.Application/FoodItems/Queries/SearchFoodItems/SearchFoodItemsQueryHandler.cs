using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodItems.Queries.SearchFoodItems;

public sealed class SearchFoodItemsQueryHandler(IFoodItemRepository foodItemRepository)
    : IRequestHandler<SearchFoodItemsQuery, Result<IReadOnlyList<FoodItemDto>>>
{
    public async Task<Result<IReadOnlyList<FoodItemDto>>> Handle(
        SearchFoodItemsQuery request, CancellationToken cancellationToken)
    {
        var items = string.IsNullOrWhiteSpace(request.Search)
            ? await foodItemRepository.GetAllAsync(cancellationToken)
            : await foodItemRepository.SearchByNameAsync(request.Search, cancellationToken);

        var dtos = items
            .Select(f => new FoodItemDto(
                f.Id,
                f.Name,
                f.ServingSize,
                f.Points.Value,
                f.Category?.Name ?? string.Empty))
            .ToList();

        return Result.Success<IReadOnlyList<FoodItemDto>>(dtos);
    }
}
