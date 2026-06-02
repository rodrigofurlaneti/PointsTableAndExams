using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.FoodItems.Commands.Delete;

public record DeleteFoodItemCommand(Guid Id) : IRequest<Result>;
