using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodCategories.Commands.Delete;

public sealed class DeleteFoodCategoryCommandHandler(
    IFoodCategoryRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteFoodCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteFoodCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
            return Result.Failure(new Error("NotFound", "Categoria de alimento não encontrada."));

        await repository.DeleteAsync(category, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
