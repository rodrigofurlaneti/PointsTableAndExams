using FluentAssertions;
using NSubstitute;
using PointsTableAndExams.Application.DailyLogs.Commands.CreateDailyLog;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.UnitTests.Application.DailyLogs;

public sealed class CreateDailyLogCommandHandlerTests
{
    private readonly IDailyLogRepository _repository = Substitute.For<IDailyLogRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateDailyLogCommandHandler _handler;

    public CreateDailyLogCommandHandlerTests()
    {
        _handler = new CreateDailyLogCommandHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WhenLogDoesNotExist_ShouldCreateLog()
    {
        var userId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        _repository.ExistsForDateAsync(userId, date, Arg.Any<CancellationToken>()).Returns(false);

        var result = await _handler.Handle(new CreateDailyLogCommand(userId, date, null), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _repository.Received(1).AddAsync(Arg.Any<DailyLog>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenLogAlreadyExists_ShouldReturnFailure()
    {
        var userId = Guid.NewGuid();
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        _repository.ExistsForDateAsync(userId, date, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _handler.Handle(new CreateDailyLogCommand(userId, date, null), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DailyLog.AlreadyExists");
    }
}
