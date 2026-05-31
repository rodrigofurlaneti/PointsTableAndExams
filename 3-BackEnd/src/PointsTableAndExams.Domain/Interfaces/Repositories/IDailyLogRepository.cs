using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Domain.Interfaces.Repositories;

public interface IDailyLogRepository : IRepository<DailyLog>
{
    Task<DailyLog?> GetByUserAndDateAsync(Guid userId, DateOnly date, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DailyLog>> GetHistoryAsync(Guid userId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
    Task<bool> ExistsForDateAsync(Guid userId, DateOnly date, CancellationToken cancellationToken = default);
}
