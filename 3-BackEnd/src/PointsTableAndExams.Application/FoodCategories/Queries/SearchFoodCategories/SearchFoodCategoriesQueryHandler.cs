using MediatR;
using PointsTableAndExams.Application.FoodCategories.DTOs;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodCategories.Queries.SearchFoodCategories
{
    public sealed class SearchFoodCategoriesQueryHandler(IFoodCategoryRepository foodCategoryRepository)
        : IRequestHandler<SearchFoodCategoriesQuery, Result<IReadOnlyList<FoodCategoryDto>>>
    {
        public async Task<Result<IReadOnlyList<FoodCategoryDto>>> Handle(
            SearchFoodCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = string.IsNullOrWhiteSpace(request.Search)
                ? await foodCategoryRepository.GetAllAsync(cancellationToken)
                : await foodCategoryRepository.SearchByNameAsync(request.Search, cancellationToken);

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
}
