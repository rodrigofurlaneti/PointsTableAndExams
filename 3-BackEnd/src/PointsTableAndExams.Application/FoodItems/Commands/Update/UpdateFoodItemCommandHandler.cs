using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodItems.Commands.Update
{
    public sealed class UpdateFoodItemCommandHandler(
    IFoodItemRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateFoodItemCommand, Result>
    {
        public async Task<Result> Handle(UpdateFoodItemCommand request, CancellationToken cancellationToken)
        {
            // 1. Busca a entidade a ser atualizada
            var foodItem = await repository.GetByIdAsync(request.Id, cancellationToken);

            // 2. Fail-fast: Se não encontrar, retorna falha imediatamente
            if (foodItem is null)
                return Result.Failure("Alimento não encontrado.");

            // 3. Atualização da entidade com setters privados (Rich Domain Model)
            var updateResult = foodItem.Update(
                request.Name,
                request.ServingSize,
                request.Points,
                request.Notes);

            // 4. Fail-fast: Valida se a entidade rejeitou a atualização (ex: quebra de regra de negócio)
            if (!updateResult.IsSuccess)
                return Result.Failure(updateResult.Error);

            // 5. Marca o estado como modificado no ChangeTracker do EF Core
            await repository.UpdateAsync(foodItem, cancellationToken);

            // 6. IUnitOfWork persiste as alterações no banco de forma atômica
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}
