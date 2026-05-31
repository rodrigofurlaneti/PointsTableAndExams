using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.ExamRequests.Queries.GetExamRequestById;

public sealed class GetExamRequestByIdQueryHandler(IExamRequestRepository repository)
    : IRequestHandler<GetExamRequestByIdQuery, Result<ExamRequestResponse>>
{
    public async Task<Result<ExamRequestResponse>> Handle(GetExamRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var examRequest = await repository.GetWithItemsAsync(request.RequestId, cancellationToken);
        if (examRequest is null) return Result.Failure<ExamRequestResponse>(Error.NotFound);

        var items = examRequest.Items.Select(i => new ExamRequestItemResponse(
            i.Id, i.ExamId, i.Exam?.Name ?? string.Empty, i.Exam?.Abbreviation,
            i.IsCompleted, i.CompletedDate, i.Result, i.Laboratory)).ToList();

        return Result.Success(new ExamRequestResponse(
            examRequest.Id, examRequest.UserId, examRequest.RequestDate,
            examRequest.DoctorName, examRequest.Notes, items));
    }
}
