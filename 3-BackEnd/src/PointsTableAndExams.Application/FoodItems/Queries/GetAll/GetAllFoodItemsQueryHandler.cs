using MediatR;
using PointsTableAndExams.Application.FoodItems.DTOs;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodItems.Queries.GetAll;

public sealed class GetAllFoodItemsQueryHandler(IFoodItemRepository repository)
    : IRequestHandler<GetAllFoodItemsQuery, Result<IReadOnlyList<FoodItemResponse>>>
{
    public async Task<Result<IReadOnlyList<FoodItemResponse>>> Handle(GetAllFoodItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await repository.GetAllAsync(cancellationToken);

        var dtos = items
            .Select(f => new FoodItemResponse(
                f.Id,
                f.Name,
                f.Points.Value,
                f.ServingSize,
                f.Notes,
                f.FoodCategoryId,
                f.IsActive))
            .ToList();

        return Result.Success<IReadOnlyList<FoodItemResponse>>(dtos);
    }
}
