using FluentValidation;

namespace PointsTableAndExams.Application.Exam.Commands.Update;

public sealed class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
{
    public UpdateExamCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("O ID do exame e obrigatorio.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("O nome do exame e obrigatorio.")
            .MaximumLength(150).WithMessage("O nome nao pode exceder 150 caracteres.");

        RuleFor(v => v.Abbreviation)
            .MaximumLength(50).WithMessage("A abreviacao nao pode exceder 50 caracteres.");

        RuleFor(v => v.Description)
            .MaximumLength(300).WithMessage("A descricao nao pode exceder 300 caracteres.");
    }
}
