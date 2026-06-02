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
        var exam = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (exam is null)
            return Result.Failure(new Error("NotFound", "Exame nao encontrado."));

        exam.Update(request.Name, request.Abbreviation, request.Description);

        await repository.UpdateAsync(exam, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
