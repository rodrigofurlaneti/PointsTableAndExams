using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.Exam.Commands.Create
{
    public class CreateExamCommandHandler(
        IExamRepository repository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateExamCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            // 1. Cria a entidade pelo método de fábrica (Rich Domain Model), que retorna um Result
            var examResult = Exam.Create(
                request.ExamCategoryId,
                request.Name,
                request.Abbreviation,
                request.Description);

            // 2. Fail-fast: Se a criação falhar (ex: regra de domínio violada), retorna imediatamente
            if (!examResult.IsSuccess)
                return Result.Failure<Guid>(examResult.Error);

            // 3. Persistência
            await repository.AddAsync(examResult.Value, cancellationToken);

            // O CommitAsync garante a atomicidade e despacha possíveis Domain Events
            await unitOfWork.CommitAsync(cancellationToken);

            // 4. Retorna sucesso com o ID gerado
            return Result.Success<Guid>(examResult.Value.Id);
        }
    }
}
