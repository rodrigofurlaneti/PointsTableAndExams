using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Exceptions;

namespace PointsTableAndExams.Domain.Entities;

public sealed class FoodCategory : Entity
{
    private readonly List<FoodItem> _items = [];

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int? DefaultQuotaPoints { get; private set; }
    public string? ServingUnit { get; private set; }
    public byte SortOrder { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyList<FoodItem> Items => _items.AsReadOnly();

    private FoodCategory() { }

    public static FoodCategory Create(string name, string? description, int? defaultQuotaPoints, string? servingUnit, byte sortOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Food category name cannot be empty.");

        return new FoodCategory
        {
            Name = name.Trim(),
            Description = description?.Trim(),
            DefaultQuotaPoints = defaultQuotaPoints,
            ServingUnit = servingUnit?.Trim(),
            SortOrder = sortOrder,
            IsActive = true
        };
    }

    public void Update(string name, string? description, int? defaultQuotaPoints, string? servingUnit)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Food category name cannot be empty.");

        Name = name.Trim();
        Description = description?.Trim();
        DefaultQuotaPoints = defaultQuotaPoints;
        ServingUnit = servingUnit?.Trim();
        SetUpdatedAt(DateTime.UtcNow);
    }

    public void Deactivate() { IsActive = false; SetUpdatedAt(DateTime.UtcNow); }
    public void Activate()   { IsActive = true;  SetUpdatedAt(DateTime.UtcNow); }
}
