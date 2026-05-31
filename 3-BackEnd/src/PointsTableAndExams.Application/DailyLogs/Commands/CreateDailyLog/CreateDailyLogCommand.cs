using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.DailyLogs.Commands.CreateDailyLog;

public sealed record CreateDailyLogCommand(Guid UserId, DateOnly LogDate, string? Notes)
    : IRequest<Result<Guid>>;
