using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.ExamCategory.Commands.Update;

public sealed class UpdateExamCategoryCommandHandler(
    IExamCategoryRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateExamCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateExamCategoryCommand request, CancellationToken cancellationToken)
    {
        // 1. Busca a entidade a ser atualizada
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);

        // 2. Fail-fast: Categoria não encontrada
        if (category is null)
            return Result.Failure(new Error("NotFound", "Categoria de exame não encontrada."));

        // 3. Atualiza através do modelo rico
        category.Update(request.Name);

        // 4. Persiste e comita
        await repository.UpdateAsync(category, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
