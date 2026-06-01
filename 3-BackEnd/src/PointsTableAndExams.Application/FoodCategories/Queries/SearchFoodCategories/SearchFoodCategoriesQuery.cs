using MediatR;
using PointsTableAndExams.Application.FoodCategories.DTOs;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.FoodCategories.Queries.SearchFoodCategories
{
    public sealed record SearchFoodCategoriesQuery(string? Search) : IRequest<Result<IReadOnlyList<FoodCategoryDto>>>;
}
