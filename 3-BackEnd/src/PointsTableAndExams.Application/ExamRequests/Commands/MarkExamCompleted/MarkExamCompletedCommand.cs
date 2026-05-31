using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.ExamRequests.Commands.MarkExamCompleted;

public sealed record MarkExamCompletedCommand(
    Guid ExamRequestId, Guid ItemId, DateOnly CompletedDate,
    string? Result, string? Laboratory)
    : IRequest<Result>;
