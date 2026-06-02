using MediatR;
using PointsTableAndExams.Application.Exam.DTOs;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.Exam.Queries.GetAll;

public sealed class GetAllExamQueryHandler(IExamRepository repository)
    : IRequestHandler<GetAllExamQuery, Result<IReadOnlyList<ExamDto>>>
{
    public async Task<Result<IReadOnlyList<ExamDto>>> Handle(GetAllExamQuery request, CancellationToken cancellationToken)
    {
        var exams = await repository.GetAllAsync(cancellationToken);

        var dtos = exams
            .Select(e => new ExamDto(e.Id, e.ExamCategoryId, e.Name, e.Abbreviation, e.Description, e.IsActive))
            .ToList();

        return Result.Success<IReadOnlyList<ExamDto>>(dtos);
    }
}
