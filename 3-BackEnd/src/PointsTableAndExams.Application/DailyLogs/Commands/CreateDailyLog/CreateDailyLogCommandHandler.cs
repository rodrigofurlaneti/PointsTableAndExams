using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.DailyLogs.Commands.CreateDailyLog;

public sealed class CreateDailyLogCommandHandler(
    IDailyLogRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateDailyLogCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateDailyLogCommand request, CancellationToken cancellationToken)
    {
        if (await repository.ExistsForDateAsync(request.UserId, request.LogDate, cancellationToken))
            return Result.Failure<Guid>(new Error("DailyLog.AlreadyExists", $"A log for {request.LogDate} already exists."));

        var log = DailyLog.Create(request.UserId, request.LogDate, request.Notes);
        await repository.AddAsync(log, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return Result.Success(log.Id);
    }
}
