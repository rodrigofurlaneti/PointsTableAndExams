using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.DailyLogs.Commands.AddLogItem;

public sealed record AddLogItemCommand(
    Guid DailyLogId, Guid FoodItemId,
    decimal Quantity, int PointsPerServing,
    TimeOnly? MealTime, string? Notes)
    : IRequest<Result<Guid>>;
