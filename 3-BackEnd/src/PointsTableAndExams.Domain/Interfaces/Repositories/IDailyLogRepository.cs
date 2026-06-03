using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Domain.Interfaces.Repositories;

public interface IDailyLogRepository : IRepository<DailyLog>
{
    Task<DailyLog?> GetByUserAndDateAsync(Guid userId, DateOnly date, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DailyLog>> GetHistoryAsync(Guid userId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
    Task<bool> ExistsForDateAsync(Guid userId, DateOnly date, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona um DailyLogItem diretamente e atualiza TotalPoints via ExecuteUpdateAsync,
    /// evitando problemas de concorrência do EF com owned entities.
    /// </summary>
    Task<Guid> AddItemDirectAsync(Guid dailyLogId, DailyLogItem item, CancellationToken ct = default);
}
