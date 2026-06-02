using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.Exam.Commands.Delete;

public sealed class DeleteExamCommandHandler(
    IExamRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteExamCommand, Result>
{
    public async Task<Result> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
    {
        var exam = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (exam is null)
            return Result.Failure(new Error("NotFound", "Exame não encontrado."));

        await repository.DeleteAsync(exam, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
