using FluentValidation;

namespace PointsTableAndExams.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório.");

        RuleFor(v => v.FullName)
            .NotEmpty().WithMessage("O nome completo é obrigatório.")
            .MaximumLength(200).WithMessage("O nome não pode exceder 200 caracteres.");
    }
}
