using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodCategories.Commands.Create;

public sealed class CreateFoodCategoryCommandHandler(
    IFoodCategoryRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateFoodCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateFoodCategoryCommand request, CancellationToken cancellationToken)
    {
        // 1. Cria a entidade pelo método de fábrica (Rich Domain Model)
        var categoryResult = FoodCategory.Create(
            request.Name,
            request.Description,
            request.DefaultQuotaPoints,
            request.ServingUnit,
            request.SortOrder);

        // 2. Fail-fast: se a criação falhar, retorna imediatamente
        if (!categoryResult.IsSuccess)
            return Result.Failure<Guid>(categoryResult.Error);

        // 3. Persiste e comita
        await repository.AddAsync(categoryResult.Value, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        // 4. Retorna o ID gerado
        return Result.Success<Guid>(categoryResult.Value.Id);
    }
}
