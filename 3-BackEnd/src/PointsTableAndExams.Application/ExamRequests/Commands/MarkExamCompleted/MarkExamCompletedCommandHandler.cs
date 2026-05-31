using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.ExamRequests.Commands.MarkExamCompleted;

public sealed class MarkExamCompletedCommandHandler(
    IExamRequestRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<MarkExamCompletedCommand, Result>
{
    public async Task<Result> Handle(MarkExamCompletedCommand request, CancellationToken cancellationToken)
    {
        var examRequest = await repository.GetWithItemsAsync(request.ExamRequestId, cancellationToken);
        if (examRequest is null) return Result.Failure(Error.NotFound);

        examRequest.MarkExamCompleted(request.ItemId, request.CompletedDate, request.Result, request.Laboratory);
        await repository.UpdateAsync(examRequest, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return Result.Success();
    }
}
