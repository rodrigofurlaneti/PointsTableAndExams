using FluentValidation;

namespace PointsTableAndExams.Application.FoodItems.Commands.Create
{
    public sealed class CreateFoodItemCommandValidator : AbstractValidator<CreateFoodItemCommand>
    {
        public CreateFoodItemCommandValidator()
        {
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
