using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.Exam.Commands.Delete;

public record DeleteExamCommand(Guid Id) : IRequest<Result>;
