using FluentValidation;

namespace PointsTableAndExams.Application.ExamCategory.Commands.Create;

public sealed class CreateExamCategoryCommandValidator : AbstractValidator<CreateExamCategoryCommand>
{
    public CreateExamCategoryCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("O nome da categoria de exame é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");
    }
}
