using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Exceptions;

namespace PointsTableAndExams.Domain.Entities;

public sealed class Exam : Entity
{
    public Guid ExamCategoryId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Abbreviation { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    public ExamCategory? Category { get; private set; }

    private Exam() { }

    public static Exam Create(Guid categoryId, string name, string? abbreviation, string? description)
    {
        if (categoryId == Guid.Empty)
            throw new DomainException("Category id is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Exam name cannot be empty.");

        return new Exam
        {
            ExamCategoryId = categoryId,
            Name = name.Trim(),
            Abbreviation = abbreviation?.Trim(),
            Description = description?.Trim(),
            IsActive = true
        };
    }

    public void Update(string name, string? abbreviation, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Exam name cannot be empty.");
        Name = name.Trim();
        Abbreviation = abbreviation?.Trim();
        Description = description?.Trim();
        SetUpdatedAt(DateTime.UtcNow);
    }

    public void Deactivate() { IsActive = false; SetUpdatedAt(DateTime.UtcNow); }
    public void Activate()   { IsActive = true;  SetUpdatedAt(DateTime.UtcNow); }
}
