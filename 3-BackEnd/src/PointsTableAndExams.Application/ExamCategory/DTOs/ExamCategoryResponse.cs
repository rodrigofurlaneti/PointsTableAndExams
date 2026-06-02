namespace PointsTableAndExams.Application.ExamCategory.DTOs;

public sealed record ExamCategoryResponse(
    Guid Id,
    string Name,
    byte SortOrder,
    bool IsActive);
