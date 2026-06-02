namespace PointsTableAndExams.Application.Exam.DTOs;

public sealed record ExamDto(
    Guid Id,
    Guid ExamCategoryId,
    string Name,
    string? Abbreviation,
    string? Description,
    bool IsActive);
