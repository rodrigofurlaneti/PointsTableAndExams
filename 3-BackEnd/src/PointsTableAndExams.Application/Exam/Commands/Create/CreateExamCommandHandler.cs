using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using ExamEntity = PointsTableAndExams.Domain.Entities.Exam;

namespace PointsTableAndExams.Application.Exam.Commands.Create;

public sealed class CreateExamCommandHandler(
    IExamRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateExamCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateExamCommand request, CancellationToken cancellationToken)
    {
        // 1. Cria a entidade pelo metodo de fabrica (lanca DomainException se invalido)
        var exam = ExamEntity.Create(
            request.ExamCategoryId,
            request.Name,
            request.Abbreviation,
            request.Description);

        // 2. Persiste e comita
        await repository.AddAsync(exam, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        // 3. Retorna o ID gerado
        return Result.Success<Guid>(exam.Id);
    }
}
