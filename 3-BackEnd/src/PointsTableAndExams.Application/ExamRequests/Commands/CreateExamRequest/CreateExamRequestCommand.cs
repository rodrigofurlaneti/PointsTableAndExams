using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.ExamRequests.Commands.CreateExamRequest;

public sealed record CreateExamRequestCommand(
    Guid UserId, DateOnly RequestDate, string? DoctorName, string? Notes, IReadOnlyList<Guid> ExamIds)
    : IRequest<Result<Guid>>;
