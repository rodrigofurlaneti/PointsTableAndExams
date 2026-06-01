using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.FoodItems.Commands.Update
{
    public sealed record UpdateFoodItemCommand(
        Guid Id,
        Guid CategoryId,
        string Name,
        string? ServingSize,
        int Points,
        string? Notes) : IRequest<Result>;
}
