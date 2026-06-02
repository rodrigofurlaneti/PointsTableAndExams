using MediatR;
using PointsTableAndExams.Application.ExamCategory.DTOs;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.ExamCategory.Queries.GetById;

public sealed class GetExamCategoryByIdQueryHandler(IExamCategoryRepository repository)
    : IRequestHandler<GetExamCategoryByIdQuery, Result<ExamCategoryResponse>>
{
    public async Task<Result<ExamCategoryResponse>> Handle(GetExamCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Busca a entidade
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);

        // 2. Fail-fast: não encontrada
        if (category is null)
            return Result<ExamCategoryResponse>.Failure(new Error("NotFound", "Categoria de exame não encontrada."));

        // 3. Mapeia para DTO
        var response = new ExamCategoryResponse(
            category.Id,
            category.Name,
            category.SortOrder,
            category.IsActive);

        return Result<ExamCategoryResponse>.Success(response);
    }
}
