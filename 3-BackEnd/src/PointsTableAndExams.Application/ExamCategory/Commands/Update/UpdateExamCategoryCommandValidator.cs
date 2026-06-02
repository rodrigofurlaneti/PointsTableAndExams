using FluentValidation;

namespace PointsTableAndExams.Application.ExamCategory.Commands.Update;

public sealed class UpdateExamCategoryCommandValidator : AbstractValidator<UpdateExamCategoryCommand>
{
    public UpdateExamCategoryCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("O ID da categoria de exame é obrigatório.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("O nome da categoria de exame é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");
    }
}
