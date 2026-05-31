using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Domain.DomainEvents;

public sealed record ExamCompletedEvent(Guid EventId, DateTime OccurredAt, Guid RequestItemId, Guid ExamId) : IDomainEvent
{
    public static ExamCompletedEvent Create(Guid requestItemId, Guid examId) =>
        new(Guid.NewGuid(), DateTime.UtcNow, requestItemId, examId);
}
