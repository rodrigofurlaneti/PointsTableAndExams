using MediatR;
using PointsTableAndExams.Application.Exam.DTOs;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.Exam.Queries.GetById;

public sealed class GetExamByIdQueryHandler(IExamRepository repository)
    : IRequestHandler<GetExamByIdQuery, Result<ExamResponse>>
{
    public async Task<Result<ExamResponse>> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
    {
        var exam = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (exam is null)
            return Result<ExamResponse>.Failure(new Error("NotFound", "Exame não encontrado."));

        var response = new ExamResponse(
            exam.Id,
            exam.ExamCategoryId,
            exam.Name,
            exam.Abbreviation,
            exam.Description,
            exam.IsActive);

        return Result<ExamResponse>.Success(response);
    }
}
