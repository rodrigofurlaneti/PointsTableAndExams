using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Exceptions;

namespace PointsTableAndExams.Domain.Entities;

public sealed class DailyLogItem : Entity
{
    public Guid DailyLogId { get; private set; }
    public Guid FoodItemId { get; private set; }
    public decimal Quantity { get; private set; }
    public int PointsComputed { get; private set; }
    public TimeOnly? MealTime { get; private set; }
    public string? Notes { get; private set; }

    public FoodItem? FoodItem { get; private set; }

    private DailyLogItem() { }

    // Usado internamente pelo aggregate DailyLog
    internal static DailyLogItem Create(Guid logId, Guid foodItemId, decimal quantity, int pointsPerServing, TimeOnly? mealTime, string? notes) =>
        CreateForLog(logId, foodItemId, quantity, pointsPerServing, mealTime, notes);

    // Público para uso direto pela Application layer (bypass aggregate)
    public static DailyLogItem CreateForLog(Guid logId, Guid foodItemId, decimal quantity, int pointsPerServing, TimeOnly? mealTime, string? notes)
    {
        if (foodItemId == Guid.Empty) throw new DomainException("Food item id is required.");
        if (quantity <= 0) throw new DomainException("Quantity must be greater than zero.");

        return new DailyLogItem
        {
            DailyLogId = logId,
            FoodItemId = foodItemId,
            Quantity = quantity,
            PointsComputed = (int)Math.Round(quantity * pointsPerServing),
            MealTime = mealTime,
            Notes = notes?.Trim()
        };
    }
}
