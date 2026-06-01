using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointsTableAndExams.Application.FoodItems.Commands.Update
{
    public sealed class UpdateFoodItemCommandValidator : AbstractValidator<UpdateFoodItemCommand>
    {
        public UpdateFoodItemCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("O ID do alimento é obrigatório.");

            RuleFor(v => v.CategoryId)
                .NotEmpty().WithMessage("O ID da categoria é obrigatório.");

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("O nome do alimento é obrigatório.")
                .MaximumLength(150).WithMessage("O nome não pode exceder 150 caracteres.");

            RuleFor(v => v.ServingSize)
                .MaximumLength(100).WithMessage("A porção (ServingSize) não pode exceder 100 caracteres.");

            RuleFor(v => v.Notes)
                .MaximumLength(300).WithMessage("As observações não podem exceder 300 caracteres.");
        }
    }
}
