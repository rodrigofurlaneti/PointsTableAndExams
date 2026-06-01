namespace PointsTableAndExams.Application.FoodCategories.DTOs
{
    public sealed record FoodCategoryDto(
        Guid Id,
        string Name,
        string? Description,
        int? DefaultQuotaPoints,
        string? ServingUnit,
        byte SortOrder,
        bool IsActive);
}
