using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointsTableAndExams.Application.Exam.Commands.Create
    public class CreateExamCommandValidator : AbstractValidator<CreateExamCommand>
    {
        public CreateExamCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("O nome do exame é obrigatório.")
                .MaximumLength(150).WithMessage("O nome não pode exceder 150 caracteres.");

            RuleFor(v => v.Abbreviation)
                .MaximumLength(50).WithMessage("A abreviação não pode exceder 50 caracteres.");

            RuleFor(v => v.Description)
                .MaximumLength(300).WithMessage("A descrição não pode exceder 300 caracteres.");
        }
    }
}
