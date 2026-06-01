using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Application.FoodCategories.DTOs;

namespace PointsTableAndExams.Application.FoodCategories.Queries.GetById
{
    public record GetFoodCategoryByIdQuery(Guid Id) : IRequest<Result<FoodCategoryResponse>>;
}
