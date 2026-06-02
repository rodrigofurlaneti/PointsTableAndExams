using MediatR;
using PointsTableAndExams.Application.FoodItems.DTOs;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.FoodItems.Queries.GetAll;

public record GetAllFoodItemsQuery() : IRequest<Result<IReadOnlyList<FoodItemResponse>>>;
