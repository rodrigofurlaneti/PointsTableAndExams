using FluentAssertions;
using NSubstitute;
using PointsTableAndExams.Application.DailyLogs.Commands.AddLogItem;
using PointsTableAndExams.Application.DailyLogs.Commands.CreateDailyLog;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using Reqnroll;

namespace PointsTableAndExams.BddTests.StepDefinitions;

[Binding]
public sealed class DailyLogSteps
{
    private readonly IDailyLogRepository _repository = Substitute.For<IDailyLogRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private Guid _userId = Guid.NewGuid();
    private Guid _logId;
    private DailyLog? _log;
    private Result<Guid>? _result;
    private DateOnly _today = DateOnly.FromDateTime(DateTime.UtcNow);

    [Given("I am a registered user")]
    public void GivenIAmARegisteredUser() { /* user already exists in domain */ }

    [Given("I already have a log for today")]
    public void GivenIAlreadyHaveALogForToday() =>
        _repository.ExistsForDateAsync(_userId, _today, Arg.Any<CancellationToken>()).Returns(true);

    [Given("I have a daily log for today")]
    public async Task GivenIHaveADailyLogForToday()
    {
        _repository.ExistsForDateAsync(_userId, _today, Arg.Any<CancellationToken>()).Returns(false);
        var handler = new CreateDailyLogCommandHandler(_repository, _unitOfWork);
        _result = await handler.Handle(new CreateDailyLogCommand(_userId, _today, null), CancellationToken.None);
        _logId = _result!.Value;

        _log = DailyLog.Create(_userId, _today);
        _repository.GetByIdAsync(_logId, Arg.Any<CancellationToken>()).Returns(_log);
    }

    [When("I create a daily log for today")]
    public async Task WhenICreateADailyLogForToday()
    {
        _repository.ExistsForDateAsync(_userId, _today, Arg.Any<CancellationToken>()).Returns(false);
        var handler = new CreateDailyLogCommandHandler(_repository, _unitOfWork);
        _result = await handler.Handle(new CreateDailyLogCommand(_userId, _today, null), CancellationToken.None);
    }

    [When("I try to create another log for today")]
    public async Task WhenITryToCreateAnotherLogForToday()
    {
        var handler = new CreateDailyLogCommandHandler(_repository, _unitOfWork);
        _result = await handler.Handle(new CreateDailyLogCommand(_userId, _today, null), CancellationToken.None);
    }

    [When("I add a food item with {int} servings of {int} points each")]
    public async Task WhenIAddAFoodItemWithServings(int quantity, int pointsPerServing)
    {
        var handler = new AddLogItemCommandHandler(_repository, _unitOfWork);
        _result = await handler.Handle(
            new AddLogItemCommand(_logId, Guid.NewGuid(), quantity, pointsPerServing, null, null),
            CancellationToken.None);
    }

    [Then("the daily log should be created successfully")]
    public void ThenTheDailyLogShouldBeCreatedSuccessfully() =>
        _result!.IsSuccess.Should().BeTrue();

    [Then("the operation should fail with {string}")]
    public void ThenTheOperationShouldFailWith(string errorCode)
    {
        _result!.IsFailure.Should().BeTrue();
        _result.Error.Code.Should().Be(errorCode);
    }

    [Then("the log total should be {int} points")]
    public void ThenTheLogTotalShouldBePoints(int expected) =>
        _log!.TotalPoints.Value.Should().Be(expected);
}
