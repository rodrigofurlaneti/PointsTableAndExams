using FluentValidation;

namespace PointsTableAndExams.Application.DailyLogs.Commands.CreateDailyLog;

public sealed class CreateDailyLogCommandValidator : AbstractValidator<CreateDailyLogCommand>
{
    public CreateDailyLogCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.LogDate).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Log date cannot be in the future.");
    }
}
