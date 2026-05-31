using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.DomainEvents;
using PointsTableAndExams.Domain.Exceptions;
using PointsTableAndExams.Domain.ValueObjects;

namespace PointsTableAndExams.Domain.Entities;

public sealed class DailyLog : AggregateRoot
{
    private readonly List<DailyLogItem> _items = [];

    public Guid UserId { get; private set; }
    public DateOnly LogDate { get; private set; }
    public Points TotalPoints { get; private set; } = Points.Zero;
    public string? Notes { get; private set; }

    public IReadOnlyList<DailyLogItem> Items => _items.AsReadOnly();

    private DailyLog() { }

    public static DailyLog Create(Guid userId, DateOnly date, string? notes = null)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User id is required.");

        var log = new DailyLog
        {
            UserId = userId,
            LogDate = date,
            TotalPoints = Points.Zero,
            Notes = notes?.Trim()
        };

        log.SetCreatedAt(DateTime.UtcNow);
        log.RaiseDomainEvent(DailyLogCreatedEvent.Create(log.Id, userId, date));

        return log;
    }

    public DailyLogItem AddItem(Guid foodItemId, decimal quantity, int pointsPerServing, TimeOnly? mealTime = null, string? notes = null)
    {
        if (foodItemId == Guid.Empty)
            throw new DomainException("Food item id is required.");

        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        var item = DailyLogItem.Create(Id, foodItemId, quantity, pointsPerServing, mealTime, notes);
        _items.Add(item);
        RecalculateTotal();
        SetUpdatedAt(DateTime.UtcNow);
        return item;
    }

    public void RemoveItem(Guid itemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId)
            ?? throw new DomainException($"Item {itemId} not found in this log.");
        _items.Remove(item);
        RecalculateTotal();
        SetUpdatedAt(DateTime.UtcNow);
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes?.Trim();
        SetUpdatedAt(DateTime.UtcNow);
    }

    private void RecalculateTotal() =>
        TotalPoints = Points.Create(_items.Sum(i => i.PointsComputed));
}
