using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.DailyLogs.Commands.AddLogItem;

public sealed class AddLogItemCommandHandler(IDailyLogRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<AddLogItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddLogItemCommand request, CancellationToken cancellationToken)
    {
        var log = await repository.GetByIdAsync(request.DailyLogId, cancellationToken);
        if (log is null) return Result.Failure<Guid>(Error.NotFound);

        var item = log.AddItem(request.FoodItemId, request.Quantity, request.PointsPerServing, request.MealTime, request.Notes);
        await repository.UpdateAsync(log, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return Result.Success(item.Id);
    }
}
