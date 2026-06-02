using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.ExamCategory.Commands.Create;

public record CreateExamCategoryCommand(
    string Name,
    byte SortOrder) : IRequest<Result<Guid>>;
