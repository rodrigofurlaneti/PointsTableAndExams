using MediatR;
using PointsTableAndExams.Application.DailyLogs.Queries.GetDailyLogByDate;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.DailyLogs.Queries.GetHistory;

public record GetDailyLogHistoryQuery(
    Guid UserId,
    DateOnly From,
    DateOnly To) : IRequest<Result<IReadOnlyList<DailyLogResponse>>>;
