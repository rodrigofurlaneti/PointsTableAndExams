using FluentValidation;

namespace PointsTableAndExams.Application.FoodCategories.Commands.Create
{
    public class CreateFoodCategoryCommandValidator : AbstractValidator<CreateFoodCategoryCommand>
    {
        public CreateFoodCategoryCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
                .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");

            RuleFor(v => v.Description)
                .MaximumLength(300).WithMessage("A descrição não pode exceder 300 caracteres.");

            RuleFor(v => v.ServingUnit)
                .MaximumLength(100).WithMessage("A unidade de medida não pode exceder 100 caracteres.");
        }
    }
}
