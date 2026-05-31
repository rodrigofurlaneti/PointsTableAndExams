using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Domain.Interfaces.Repositories;

public interface IExamRequestRepository : IRepository<ExamRequest>
{
    Task<IReadOnlyList<ExamRequest>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ExamRequest?> GetWithItemsAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ExamRequest>> GetPendingByUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
