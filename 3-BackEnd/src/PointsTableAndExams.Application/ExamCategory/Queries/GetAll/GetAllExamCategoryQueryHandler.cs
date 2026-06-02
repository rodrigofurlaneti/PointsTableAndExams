using MediatR;
using PointsTableAndExams.Application.ExamCategory.DTOs;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.ExamCategory.Queries.GetAll;

public sealed class GetAllExamCategoryQueryHandler(IExamCategoryRepository repository)
    : IRequestHandler<GetAllExamCategoryQuery, Result<IReadOnlyList<ExamCategoryDto>>>
{
    public async Task<Result<IReadOnlyList<ExamCategoryDto>>> Handle(GetAllExamCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = await repository.GetAllAsync(cancellationToken);

        var dtos = categories
            .Select(c => new ExamCategoryDto(c.Id, c.Name, c.SortOrder, c.IsActive))
            .ToList();

        return Result.Success<IReadOnlyList<ExamCategoryDto>>(dtos);
    }
}
