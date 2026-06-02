using MediatR;
using PointsTableAndExams.Application.ExamRequests.Queries.GetExamRequestById;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.ExamRequests.Queries.GetAll;

public record GetAllExamRequestsQuery(Guid? UserId = null) : IRequest<Result<IReadOnlyList<ExamRequestResponse>>>;
