using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Exceptions;
using PointsTableAndExams.Domain.ValueObjects;

namespace PointsTableAndExams.Domain.Entities;

public sealed class FoodItem : Entity
{
    public Guid FoodCategoryId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? ServingSize { get; private set; }
    public Points Points { get; private set; } = ValueObjects.Points.Zero;
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; }

    public FoodCategory? Category { get; private set; }

    private FoodItem() { }

    public static FoodItem Create(Guid categoryId, string name, string? servingSize, int points, string? notes = null)
    {
        if (categoryId == Guid.Empty)
            throw new DomainException("Category id is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Food item name cannot be empty.");

        return new FoodItem
        {
            FoodCategoryId = categoryId,
            Name = name.Trim(),
            ServingSize = servingSize?.Trim(),
            Points = ValueObjects.Points.Create(points),
            Notes = notes?.Trim(),
            IsActive = true
        };
    }

    public void Update(string name, string? servingSize, int points, string? notes)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Food item name cannot be empty.");

        Name = name.Trim();
        ServingSize = servingSize?.Trim();
        Points = ValueObjects.Points.Create(points);
        Notes = notes?.Trim();
        SetUpdatedAt(DateTime.UtcNow);
    }

    public void Deactivate() { IsActive = false; SetUpdatedAt(DateTime.UtcNow); }
    public void Activate()   { IsActive = true;  SetUpdatedAt(DateTime.UtcNow); }
}
