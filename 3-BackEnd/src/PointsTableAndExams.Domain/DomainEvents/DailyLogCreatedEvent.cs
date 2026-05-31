using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Domain.DomainEvents;

public sealed record DailyLogCreatedEvent(Guid EventId, DateTime OccurredAt, Guid LogId, Guid UserId, DateOnly LogDate) : IDomainEvent
{
    public static DailyLogCreatedEvent Create(Guid logId, Guid userId, DateOnly date) =>
        new(Guid.NewGuid(), DateTime.UtcNow, logId, userId, date);
}
