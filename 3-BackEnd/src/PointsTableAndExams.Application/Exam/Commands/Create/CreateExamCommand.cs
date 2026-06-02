using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.Exam.Commands.Create
{
    public record CreateExamCommand(
        Guid ExamCategoryId,
        string Name,
        string? Abbreviation,
        string? Description) : IRequest<Result<Guid>>;
}
