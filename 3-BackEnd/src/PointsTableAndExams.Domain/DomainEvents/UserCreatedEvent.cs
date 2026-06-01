using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Domain.DomainEvents;

public sealed record UserCreatedEvent(Guid EventId, DateTime OccurredAt, Guid UserId, string Email) : IDomainEvent
{
    public static UserCreatedEvent Create(Guid userId, string email) =>
        new(Guid.NewGuid(), DateTime.Now, userId, email);
}
