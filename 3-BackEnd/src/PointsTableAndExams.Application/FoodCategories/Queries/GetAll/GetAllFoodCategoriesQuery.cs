using MediatR;
using PointsTableAndExams.Application.FoodCategories.DTOs;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.FoodCategories.Queries.GetAll;

public record GetAllFoodCategoriesQuery() : IRequest<Result<IReadOnlyList<FoodCategoryDto>>>;
