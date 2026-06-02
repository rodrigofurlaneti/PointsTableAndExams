using MediatR;
using PointsTableAndExams.Application.Exam.Commands.Update;
using PointsTableAndExams.Application.FoodCategories.Commands.Update;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.FoodCategories.Commands.Create
{
    public class UpdateExamCommandHandler(
        IExamRepository repository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateExamCommand, Result>
    {
        public async Task<Result> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            // 1. Busca a entidade a ser atualizada
            var exam = await repository.GetByIdAsync(request.Id, cancellationToken);

            // 2. Fail-fast: Exame não encontrada (sem o uso de 'else')
            if (exam is null)
                return Result.Failure("Exame de alimento não encontrada.");

            // 3. Atualiza através do modelo rico, que também nos devolve um Result
            var updateResult = exam.Update(
                request.Id,
                request.ExamCategoryId,
                request.Name,
                request.Abbreviation,
                request.Description);

            if (!updateResult.IsSuccess)
                return Result.Failure(updateResult.Error);

            // 4. Persiste a alteração na infraestrutura e aciona o Commit
            await repository.UpdateAsync(exam, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            // 5. Retorna sucesso vazio
            return Result.Success();
        }
    }

