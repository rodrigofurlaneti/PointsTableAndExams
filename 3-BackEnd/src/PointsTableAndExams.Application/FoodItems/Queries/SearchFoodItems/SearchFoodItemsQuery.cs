using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.FoodItems.Queries.SearchFoodItems;

public sealed record SearchFoodItemsQuery(string? Search) : IRequest<Result<IReadOnlyList<FoodItemDto>>>;

public sealed record FoodItemDto(
    Guid Id,
    string Name,
    string? ServingSize,
    int Points,
    string Category);
