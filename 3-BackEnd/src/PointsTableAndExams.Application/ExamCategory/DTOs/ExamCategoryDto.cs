namespace PointsTableAndExams.Application.ExamCategory.DTOs;

public sealed record ExamCategoryDto(
    Guid Id,
    string Name,
    byte SortOrder,
    bool IsActive);
