using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Domain.Interfaces.Repositories;

public interface IExamCategoryRepository : IRepository<ExamCategory>
{
    Task<IReadOnlyList<ExamCategory>> GetActiveWithExamsAsync(CancellationToken cancellationToken = default);
}
