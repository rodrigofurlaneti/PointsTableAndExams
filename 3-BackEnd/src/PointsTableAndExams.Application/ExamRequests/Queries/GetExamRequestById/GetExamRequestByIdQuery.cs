using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.ExamRequests.Queries.GetExamRequestById;

public sealed record GetExamRequestByIdQuery(Guid RequestId) : IRequest<Result<ExamRequestResponse>>;

public sealed record ExamRequestResponse(
    Guid Id, Guid UserId, DateOnly RequestDate, string? DoctorName,
    string? Notes, IReadOnlyList<ExamRequestItemResponse> Items);

public sealed record ExamRequestItemResponse(
    Guid Id, Guid ExamId, string ExamName, string? Abbreviation,
    bool IsCompleted, DateOnly? CompletedDate, string? Result, string? Laboratory);
