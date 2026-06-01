using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodItems.Commands.Create;

public sealed class CreateFoodItemCommandHandler(
    IFoodItemRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateFoodItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateFoodItemCommand request, CancellationToken cancellationToken)
    {
        // 1. Cria a entidade pelo método de fábrica, que agora retorna um Result
        var foodItemResult = FoodItem.Create(
            request.CategoryId,
            request.Name,
            request.ServingSize,
            request.Points,
            request.Notes);

        // 2. Fail-fast: Se a criação falhar por violação de regra de domínio, devolve a falha
        if (!foodItemResult.IsSuccess)
            return Result<Guid>.Failure(foodItemResult.Error);

        // 3. Usa a propriedade .Value para acessar a entidade validada e persistir
        await repository.AddAsync(foodItemResult.Value, cancellationToken);

        // 4. IUnitOfWork persiste e despacha Domain Events atomicamente
        await unitOfWork.CommitAsync(cancellationToken);

        // 5. Retorna sucesso com o ID gerado
        return Result<Guid>.Success(foodItemResult.Value.Id);
    }
}