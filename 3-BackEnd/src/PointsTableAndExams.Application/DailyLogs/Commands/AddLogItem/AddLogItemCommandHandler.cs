using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.DailyLogs.Commands.AddLogItem;

public sealed class AddLogItemCommandHandler(IDailyLogRepository repository)
    : IRequestHandler<AddLogItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddLogItemCommand request, CancellationToken cancellationToken)
    {
        // Verifica se o log existe
        var exists = await repository.GetByIdAsync(request.DailyLogId, CancellationToken.None);
        if (exists is null) return Result.Failure<Guid>(Error.NotFound);

        // Cria o item via domain (validações) mas persiste diretamente
        var item = DailyLogItem.CreateForLog(
            request.DailyLogId,
            request.FoodItemId,
            request.Quantity,
            request.PointsPerServing,
            request.MealTime,
            request.Notes);

        var id = await repository.AddItemDirectAsync(request.DailyLogId, item, CancellationToken.None);
        return Result.Success(id);
    }
}
