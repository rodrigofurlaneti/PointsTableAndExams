using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.Exam.Commands.Update;

public sealed class UpdateExamCommandHandler(
    IExamRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateExamCommand, Result>
{
    public async Task<Result> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
    {
        // 1. Busca a entidade a ser atualizada
        var exam = await repository.GetByIdAsync(request.Id, cancellationToken);

        // 2. Fail-fast: Exame não encontrado
        if (exam is null)
            return Result.Failure(new Error("NotFound", "Exame não encontrado."));

        // 3. Atualiza através do modelo rico
        exam.Update(request.Name, request.Abbreviation, request.Description);

        // 4. Persiste e comita
        await repository.UpdateAsync(exam, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
