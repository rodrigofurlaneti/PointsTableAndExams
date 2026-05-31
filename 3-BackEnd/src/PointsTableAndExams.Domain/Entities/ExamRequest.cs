using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.DomainEvents;
using PointsTableAndExams.Domain.Exceptions;

namespace PointsTableAndExams.Domain.Entities;

public sealed class ExamRequest : AggregateRoot
{
    private readonly List<ExamRequestItem> _items = [];

    public Guid UserId { get; private set; }
    public DateOnly RequestDate { get; private set; }
    public string? DoctorName { get; private set; }
    public string? Notes { get; private set; }

    public IReadOnlyList<ExamRequestItem> Items => _items.AsReadOnly();

    private ExamRequest() { }

    public static ExamRequest Create(Guid userId, DateOnly requestDate, string? doctorName, string? notes)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User id is required.");

        var request = new ExamRequest
        {
            UserId = userId,
            RequestDate = requestDate,
            DoctorName = doctorName?.Trim(),
            Notes = notes?.Trim()
        };

        request.SetCreatedAt(DateTime.UtcNow);
        request.RaiseDomainEvent(ExamRequestCreatedEvent.Create(request.Id, userId));

        return request;
    }

    public ExamRequestItem AddExam(Guid examId)
    {
        if (_items.Any(i => i.ExamId == examId))
            throw new DomainException("Exam already added to this request.");

        var item = ExamRequestItem.Create(Id, examId);
        _items.Add(item);
        SetUpdatedAt(DateTime.UtcNow);
        return item;
    }

    public void MarkExamCompleted(Guid itemId, DateOnly completedDate, string? result, string? laboratory)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new DomainException($"Exam request item {itemId} not found.");

        item.Complete(completedDate, result, laboratory);
        RaiseDomainEvent(ExamCompletedEvent.Create(item.Id, item.ExamId));
        SetUpdatedAt(DateTime.UtcNow);
    }

    public void Update(string? doctorName, string? notes)
    {
        DoctorName = doctorName?.Trim();
        Notes = notes?.Trim();
        SetUpdatedAt(DateTime.UtcNow);
    }
}
