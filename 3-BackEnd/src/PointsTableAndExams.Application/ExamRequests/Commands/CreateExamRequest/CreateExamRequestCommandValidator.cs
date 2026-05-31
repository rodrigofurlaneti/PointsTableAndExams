using FluentValidation;

namespace PointsTableAndExams.Application.ExamRequests.Commands.CreateExamRequest;

public sealed class CreateExamRequestCommandValidator : AbstractValidator<CreateExamRequestCommand>
{
    public CreateExamRequestCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ExamIds).NotEmpty().WithMessage("At least one exam must be selected.");
        RuleFor(x => x.DoctorName).MaximumLength(150).When(x => x.DoctorName is not null);
    }
}
