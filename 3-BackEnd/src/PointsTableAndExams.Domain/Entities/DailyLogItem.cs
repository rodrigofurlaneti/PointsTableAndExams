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

    internal static DailyLogItem Create(Guid logId, Guid foodItemId, decimal quantity, int pointsPerServing, TimeOnly? mealTime, string? notes)
    {
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
