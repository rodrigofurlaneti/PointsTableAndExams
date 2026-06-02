using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.ExamCategory.Commands.Update;

public record UpdateExamCategoryCommand(
    Guid Id,
    string Name) : IRequest<Result>;
