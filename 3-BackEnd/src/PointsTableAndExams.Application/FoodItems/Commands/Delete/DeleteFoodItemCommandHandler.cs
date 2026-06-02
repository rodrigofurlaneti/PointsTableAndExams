using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodItems.Commands.Delete;

public sealed class DeleteFoodItemCommandHandler(
    IFoodItemRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteFoodItemCommand, Result>
{
    public async Task<Result> Handle(DeleteFoodItemCommand request, CancellationToken cancellationToken)
    {
        var foodItem = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (foodItem is null)
            return Result.Failure(new Error("NotFound", "Alimento não encontrado."));

        await repository.DeleteAsync(foodItem, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
