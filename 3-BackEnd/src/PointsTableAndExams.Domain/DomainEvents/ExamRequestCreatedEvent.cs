using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Domain.DomainEvents;

public sealed record ExamRequestCreatedEvent(Guid EventId, DateTime OccurredAt, Guid RequestId, Guid UserId) : IDomainEvent
{
    public static ExamRequestCreatedEvent Create(Guid requestId, Guid userId) =>
        new(Guid.NewGuid(), DateTime.Now, requestId, userId);
}
