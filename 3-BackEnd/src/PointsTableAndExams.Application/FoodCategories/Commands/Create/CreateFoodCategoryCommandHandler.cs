using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodCategories.Commands.Create
{
    public class CreateFoodCategoryCommandHandler(
        IFoodCategoryRepository repository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateFoodCategoryCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateFoodCategoryCommand request, CancellationToken cancellationToken)
        {
            // 1. Cria a entidade pelo método de fábrica (Rich Domain Model), que retorna um Result
            var categoryResult = FoodCategory.Create(
                request.Name,
                request.Description,
                request.DefaultQuotaPoints,
                request.ServingUnit,
                request.SortOrder);

            // 2. Fail-fast: Se a criação falhar (ex: regra de domínio violada), retorna imediatamente
            if (!categoryResult.IsSuccess)
                return Result<Guid>.Failure(categoryResult.Error);

            // 3. Persistência
            await repository.AddAsync(categoryResult.Value, cancellationToken);

            // O CommitAsync garante a atomicidade e despacha possíveis Domain Events
            await unitOfWork.CommitAsync(cancellationToken);

            // 4. Retorna sucesso com o ID gerado
            return Result<Guid>.Success(categoryResult.Value.Id);
        }
    }
}
