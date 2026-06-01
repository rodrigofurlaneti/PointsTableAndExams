namespace PointsTableAndExams.Application.FoodItems.DTOs;

public sealed record FoodItemResponse(
    Guid Id,
    string Name,
    int Points,
    string? ServingSize,
    string? Notes,
    Guid FoodCategoryId,
    bool IsActive);