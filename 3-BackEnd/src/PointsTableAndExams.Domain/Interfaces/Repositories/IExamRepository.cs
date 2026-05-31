using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Domain.Interfaces.Repositories;

public interface IExamRepository : IRepository<Exam>
{
    Task<IReadOnlyList<Exam>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Exam>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
}
