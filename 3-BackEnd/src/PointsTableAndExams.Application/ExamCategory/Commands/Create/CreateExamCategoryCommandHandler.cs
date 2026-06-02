using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using ExamCategoryEntity = PointsTableAndExams.Domain.Entities.ExamCategory;

namespace PointsTableAndExams.Application.ExamCategory.Commands.Create;

public sealed class CreateExamCategoryCommandHandler(
    IExamCategoryRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateExamCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateExamCategoryCommand request, CancellationToken cancellationToken)
    {
        // 1. Cria a entidade pelo método de fábrica (Rich Domain Model)
        var category = ExamCategoryEntity.Create(request.Name, request.SortOrder);

        // 2. Persiste e comita
        await repository.AddAsync(category, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        // 3. Retorna o ID gerado
        return Result.Success<Guid>(category.Id);
    }
}
