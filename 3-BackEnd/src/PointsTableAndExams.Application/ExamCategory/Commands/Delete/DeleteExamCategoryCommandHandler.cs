using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.ExamCategory.Commands.Delete;

public sealed class DeleteExamCategoryCommandHandler(
    IExamCategoryRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteExamCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteExamCategoryCommand request, CancellationToken cancellationToken)
    {
        // 1. Busca a entidade
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);

        // 2. Fail-fast: não encontrada
        if (category is null)
            return Result.Failure(new Error("NotFound", "Categoria de exame não encontrada."));

        // 3. Remove e comita
        await repository.DeleteAsync(category, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
