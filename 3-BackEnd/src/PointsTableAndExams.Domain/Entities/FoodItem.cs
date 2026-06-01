using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.ValueObjects;
// Assumindo que você tem um namespace para o Result Pattern, ex:
// using PointsTableAndExams.Domain.Common.Models; 

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

    // O método de fábrica (Factory Method) passa a retornar Result<FoodItem>
    public static Result<FoodItem> Create(Guid categoryId, string name, string? servingSize, int points, string? notes = null)
    {
        // Object Calisthenics: Fail fast sem "else"
        if (categoryId == Guid.Empty)
            return Result<FoodItem>.Failure("Category id is required.");

        if (string.IsNullOrWhiteSpace(name))
            return Result<FoodItem>.Failure("Food item name cannot be empty.");

        // Assumindo que ValueObjects.Points.Create também retorne um Result, 
        // você precisaria validar aqui, ou usar os pontos diretamente se o VO não falhar.
        var pointsResult = ValueObjects.Points.Create(points);
        if (!pointsResult.IsSuccess)
            return Result<FoodItem>.Failure(pointsResult.Error);

        var foodItem = new FoodItem
        {
            FoodCategoryId = categoryId,
            Name = name.Trim(),
            ServingSize = servingSize?.Trim(),
            Points = pointsResult.Value,
            Notes = notes?.Trim(),
            IsActive = true
        };

        return Result<FoodItem>.Success(foodItem);
    }

    // A atualização também passa a retornar um Result
    public Result Update(string name, string? servingSize, int points, string? notes)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure("Food item name cannot be empty.");

        var pointsResult = ValueObjects.Points.Create(points);
        if (!pointsResult.IsSuccess)
            return Result.Failure(pointsResult.Error);

        Name = name.Trim();
        ServingSize = servingSize?.Trim();
        Points = pointsResult.Value;
        Notes = notes?.Trim();
        SetUpdatedAt(DateTime.Now);

        return Result.Success();
    }

    public void Deactivate() { IsActive = false; SetUpdatedAt(DateTime.Now); }
    public void Activate() { IsActive = true; SetUpdatedAt(DateTime.Now); }
}