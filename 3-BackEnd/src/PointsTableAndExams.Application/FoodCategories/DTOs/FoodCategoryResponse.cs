namespace PointsTableAndExams.Application.FoodCategories.DTOs
{
    public record FoodCategoryResponse(
        Guid Id,
        string Name,
        string? Description,
        int? DefaultQuotaPoints,
        string? ServingUnit,
        byte SortOrder,
        bool IsActive);
}
