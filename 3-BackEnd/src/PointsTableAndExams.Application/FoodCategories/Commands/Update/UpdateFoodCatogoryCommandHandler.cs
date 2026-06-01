using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodCategories.Commands.Update
{
    public class UpdateFoodCategoryCommandHandler(
    IFoodCategoryRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateFoodCategoryCommand, Result>
    {
        public async Task<Result> Handle(UpdateFoodCategoryCommand request, CancellationToken cancellationToken)
        {
            // 1. Busca a entidade a ser atualizada
            var category = await repository.GetByIdAsync(request.Id, cancellationToken);

            // 2. Fail-fast: Categoria não encontrada (sem o uso de 'else')
            if (category is null)
                return Result.Failure("Categoria de alimento não encontrada.");

            // 3. Atualiza através do modelo rico, que também nos devolve um Result
            var updateResult = category.Update(
                request.Name,
                request.Description,
                request.DefaultQuotaPoints,
                request.ServingUnit);

            if (!updateResult.IsSuccess)
                return Result.Failure(updateResult.Error);

            // 4. Persiste a alteração na infraestrutura e aciona o Commit
            await repository.UpdateAsync(category, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            // 5. Retorna sucesso vazio
            return Result.Success();
        }
    }
}
