using PointsTableAndExams.Domain.Common;
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

    public static Result<FoodItem> Create(Guid categoryId, string name, string? servingSize, int points, string? notes = null)
    {
        if (categoryId == Guid.Empty)
            return Result.Failure<FoodItem>(new Error("Validation.Required", "Category id is required."));

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<FoodItem>(new Error("Validation.Required", "Food item name cannot be empty."));

        // Points.Create lança DomainException se inválido (não retorna Result)
        var pointsVO = ValueObjects.Points.Create(points);

        var foodItem = new FoodItem
        {
            FoodCategoryId = categoryId,
            Name = name.Trim(),
            ServingSize = servingSize?.Trim(),
            Points = pointsVO,
            Notes = notes?.Trim(),
            IsActive = true
        };

        return Result.Success<FoodItem>(foodItem);
    }

    public Result Update(string name, string? servingSize, int points, string? notes)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(new Error("Validation.Required", "Food item name cannot be empty."));

        // Points.Create lança DomainException se inválido (não retorna Result)
        var pointsVO = ValueObjects.Points.Create(points);

        Name = name.Trim();
        ServingSize = servingSize?.Trim();
        Points = pointsVO;
        Notes = notes?.Trim();
        SetUpdatedAt(DateTime.Now);

        return Result.Success();
    }

    public void Deactivate() { IsActive = false; SetUpdatedAt(DateTime.Now); }
    public void Activate() { IsActive = true; SetUpdatedAt(DateTime.Now); }
}
