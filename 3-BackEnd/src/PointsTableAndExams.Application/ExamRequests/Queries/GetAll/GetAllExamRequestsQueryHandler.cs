using MediatR;
using PointsTableAndExams.Application.ExamRequests.Queries.GetExamRequestById;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.ExamRequests.Queries.GetAll;

public sealed class GetAllExamRequestsQueryHandler(IExamRequestRepository repository)
    : IRequestHandler<GetAllExamRequestsQuery, Result<IReadOnlyList<ExamRequestResponse>>>
{
    public async Task<Result<IReadOnlyList<ExamRequestResponse>>> Handle(GetAllExamRequestsQuery request, CancellationToken cancellationToken)
    {
        var examRequests = request.UserId.HasValue
            ? await repository.GetByUserAsync(request.UserId.Value, cancellationToken)
            : await repository.GetAllAsync(cancellationToken);

        var dtos = examRequests
            .Select(er => new ExamRequestResponse(
                er.Id,
                er.UserId,
                er.RequestDate,
                er.DoctorName,
                er.Notes,
                er.Items.Select(i => new ExamRequestItemResponse(
                    i.Id, i.ExamId, i.Exam?.Name ?? string.Empty, i.Exam?.Abbreviation,
                    i.IsCompleted, i.CompletedDate, i.Result, i.Laboratory)).ToList()))
            .ToList();

        return Result.Success<IReadOnlyList<ExamRequestResponse>>(dtos);
    }
}
