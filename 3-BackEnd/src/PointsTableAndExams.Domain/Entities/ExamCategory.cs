using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Exceptions;

namespace PointsTableAndExams.Domain.Entities;

public sealed class ExamCategory : Entity
{
    private readonly List<Exam> _exams = [];

    public string Name { get; private set; } = string.Empty;
    public byte SortOrder { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyList<Exam> Exams => _exams.AsReadOnly();

    private ExamCategory() { }

    public static ExamCategory Create(string name, byte sortOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Exam category name cannot be empty.");

        return new ExamCategory { Name = name.Trim(), SortOrder = sortOrder, IsActive = true };
    }

    public void Update(string name) 
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Exam category name cannot be empty.");
        Name = name.Trim();
        SetUpdatedAt(DateTime.UtcNow);
    }

    public void Deactivate() { IsActive = false; SetUpdatedAt(DateTime.UtcNow); }
    public void Activate()   { IsActive = true;  SetUpdatedAt(DateTime.UtcNow); }
}
