using FluentValidation;

namespace PointsTableAndExams.Application.Exam.Commands.Create;

public sealed class CreateExamCommandValidator : AbstractValidator<CreateExamCommand>
{
    public CreateExamCommandValidator()
    {
        RuleFor(v => v.ExamCategoryId)
            .NotEmpty().WithMessage("O ID da categoria e obrigatorio.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("O nome do exame e obrigatorio.")
            .MaximumLength(150).WithMessage("O nome nao pode exceder 150 caracteres.");

        RuleFor(v => v.Abbreviation)
            .MaximumLength(50).WithMessage("A abreviacao nao pode exceder 50 caracteres.");

        RuleFor(v => v.Description)
            .MaximumLength(300).WithMessage("A descricao nao pode exceder 300 caracteres.");
    }
}
