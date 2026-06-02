using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.Exam.Commands.Update
{
    public record UpdateExamCommand(
    Guid Id,
    Guid ExamCategoryId,
    string Name,
    string? Abbreviation,
    string? Description) : IRequest<Result>;
}
