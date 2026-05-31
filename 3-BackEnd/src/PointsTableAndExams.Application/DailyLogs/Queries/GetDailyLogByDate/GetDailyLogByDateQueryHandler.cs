using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.DailyLogs.Queries.GetDailyLogByDate;

public sealed class GetDailyLogByDateQueryHandler(IDailyLogRepository repository)
    : IRequestHandler<GetDailyLogByDateQuery, Result<DailyLogResponse>>
{
    public async Task<Result<DailyLogResponse>> Handle(GetDailyLogByDateQuery request, CancellationToken cancellationToken)
    {
        var log = await repository.GetByUserAndDateAsync(request.UserId, request.LogDate, cancellationToken);
        if (log is null) return Result.Failure<DailyLogResponse>(Error.NotFound);

        var items = log.Items.Select(i => new DailyLogItemResponse(
            i.Id, i.FoodItemId, i.FoodItem?.Name ?? string.Empty,
            i.Quantity, i.PointsComputed, i.MealTime, i.Notes)).ToList();

        return Result.Success(new DailyLogResponse(log.Id, log.UserId, log.LogDate, log.TotalPoints, log.Notes, items));
    }
}
