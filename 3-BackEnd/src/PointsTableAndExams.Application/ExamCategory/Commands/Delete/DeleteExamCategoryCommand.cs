using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.ExamCategory.Commands.Delete;

public record DeleteExamCategoryCommand(Guid Id) : IRequest<Result>;
