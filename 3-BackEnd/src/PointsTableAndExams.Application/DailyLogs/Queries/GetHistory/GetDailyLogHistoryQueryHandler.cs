using MediatR;
using PointsTableAndExams.Application.DailyLogs.Queries.GetDailyLogByDate;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.DailyLogs.Queries.GetHistory;

public sealed class GetDailyLogHistoryQueryHandler(IDailyLogRepository repository)
    : IRequestHandler<GetDailyLogHistoryQuery, Result<IReadOnlyList<DailyLogResponse>>>
{
    public async Task<Result<IReadOnlyList<DailyLogResponse>>> Handle(GetDailyLogHistoryQuery request, CancellationToken cancellationToken)
    {
        var logs = await repository.GetHistoryAsync(request.UserId, request.From, request.To, cancellationToken);

        var dtos = logs
            .Select(log => new DailyLogResponse(
                log.Id,
                log.UserId,
                log.LogDate,
                log.TotalPoints,
                log.Notes,
                log.Items.Select(i => new DailyLogItemResponse(
                    i.Id, i.FoodItemId, i.FoodItem?.Name ?? string.Empty,
                    i.Quantity, i.PointsComputed, i.MealTime, i.Notes)).ToList()))
            .ToList();

        return Result.Success<IReadOnlyList<DailyLogResponse>>(dtos);
    }
}
