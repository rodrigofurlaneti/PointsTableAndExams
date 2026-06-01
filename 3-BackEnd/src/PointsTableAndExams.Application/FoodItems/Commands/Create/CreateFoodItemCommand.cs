using MediatR;
using PointsTableAndExams.Domain.Common;
namespace PointsTableAndExams.Application.FoodItems.Commands.Create
{
    public sealed record CreateFoodItemCommand(
        Guid CategoryId,
        string Name,
        string? ServingSize,
        int Points,
        string? Notes) : IRequest<Result<Guid>>;
}
