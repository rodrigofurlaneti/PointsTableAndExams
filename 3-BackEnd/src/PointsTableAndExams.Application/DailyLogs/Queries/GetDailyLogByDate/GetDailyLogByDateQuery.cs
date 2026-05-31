using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.DailyLogs.Queries.GetDailyLogByDate;

public sealed record GetDailyLogByDateQuery(Guid UserId, DateOnly LogDate) : IRequest<Result<DailyLogResponse>>;

public sealed record DailyLogResponse(
    Guid Id, Guid UserId, DateOnly LogDate,
    int TotalPoints, string? Notes, IReadOnlyList<DailyLogItemResponse> Items);

public sealed record DailyLogItemResponse(
    Guid Id, Guid FoodItemId, string FoodItemName,
    decimal Quantity, int PointsComputed, TimeOnly? MealTime, string? Notes);
