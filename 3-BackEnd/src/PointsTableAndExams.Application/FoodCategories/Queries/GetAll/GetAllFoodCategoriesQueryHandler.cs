using MediatR;
using PointsTableAndExams.Application.FoodCategories.DTOs;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodCategories.Queries.GetAll;

public sealed class GetAllFoodCategoriesQueryHandler(IFoodCategoryRepository repository)
    : IRequestHandler<GetAllFoodCategoriesQuery, Result<IReadOnlyList<FoodCategoryDto>>>
{
    public async Task<Result<IReadOnlyList<FoodCategoryDto>>> Handle(GetAllFoodCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await repository.GetAllAsync(cancellationToken);

        var dtos = categories
            .Select(c => new FoodCategoryDto(
                c.Id,
                c.Name,
                c.Description,
                c.DefaultQuotaPoints,
                c.ServingUnit,
                c.SortOrder,
                c.IsActive))
            .ToList();

        return Result.Success<IReadOnlyList<FoodCategoryDto>>(dtos);
    }
}
