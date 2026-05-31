using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Exceptions;

namespace PointsTableAndExams.Domain.Entities;

public sealed class ExamRequestItem : Entity
{
    public Guid ExamRequestId { get; private set; }
    public Guid ExamId { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateOnly? CompletedDate { get; private set; }
    public string? Result { get; private set; }
    public string? Laboratory { get; private set; }
    public string? Notes { get; private set; }

    public Exam? Exam { get; private set; }

    private ExamRequestItem() { }

    internal static ExamRequestItem Create(Guid requestId, Guid examId) =>
        new() { ExamRequestId = requestId, ExamId = examId, IsCompleted = false };

    internal void Complete(DateOnly completedDate, string? result, string? laboratory)
    {
        if (IsCompleted)
            throw new DomainException("Exam already completed.");

        IsCompleted = true;
        CompletedDate = completedDate;
        Result = result?.Trim();
        Laboratory = laboratory?.Trim();
        SetUpdatedAt(DateTime.UtcNow);
    }
}
