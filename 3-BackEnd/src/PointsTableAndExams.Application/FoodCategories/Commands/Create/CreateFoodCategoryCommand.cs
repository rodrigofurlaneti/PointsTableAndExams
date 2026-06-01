using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.FoodCategories.Commands.Create;
{
    public record CreateFoodCategoryCommand(
        string Name,
        string? Description,
        int? DefaultQuotaPoints,
        string? ServingUnit,
        byte SortOrder) : IRequest<Result<Guid>>;
}
