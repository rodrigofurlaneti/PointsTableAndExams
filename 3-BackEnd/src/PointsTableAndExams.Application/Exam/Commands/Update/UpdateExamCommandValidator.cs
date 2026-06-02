using FluentValidation;

namespace PointsTableAndExams.Application.Exam.Commands.Update;

public sealed class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
{
    public UpdateExamCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("O ID do exame é obrigatório.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("O nome do exame é obrigatório.")
            .MaximumLength(150).WithMessage("O nome não pode exceder 150 caracteres.");

        RuleFor(v => v.Abbreviation)
            .MaximumLength(50).WithMessage("A abreviação não pode exceder 50 caracteres.");

        RuleFor(v => v.Description)
            .MaximumLength(300).WithMessage("A descrição não pode exceder 300 caracteres.");
    }
}
