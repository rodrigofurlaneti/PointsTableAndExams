using MediatR;
using PointsTableAndExams.Application.FoodItems.DTOs;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.FoodItems.Queries.GetById
{
    public sealed record GetFoodItemByIdQuery(Guid Id) : IRequest<Result<FoodItemResponse>>;
}
