using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.FoodCategories.Commands.Update
{
    public record UpdateFoodCategoryCommand(
        Guid Id,
        string Name,
        string? Description,
        int? DefaultQuotaPoints,
        string? ServingUnit) : IRequest<Result>;
}
