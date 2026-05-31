using FluentAssertions;
using NSubstitute;
using PointsTableAndExams.Application.Common.Interfaces;
using PointsTableAndExams.Application.Users.Commands.CreateUser;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Enums;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.UnitTests.Application.Users;

public sealed class CreateUserCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _passwordHasher.Hash(Arg.Any<string>()).Returns("hashed_password");
        _handler = new CreateUserCommandHandler(_userRepository, _unitOfWork, _passwordHasher);
    }

    [Fact]
    public async Task Handle_WhenEmailAvailable_ShouldCreateUser()
    {
        _userRepository.EmailExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);
        _userRepository.UsernameExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);

        var result = await _handler.Handle(
            new CreateUserCommand("John Doe", "john@test.com", null, null, Gender.Male, "john.doe", "Password1"),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        await _userRepository.Received(1).AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenEmailTaken_ShouldReturnFailure()
    {
        _userRepository.EmailExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _handler.Handle(
            new CreateUserCommand("John Doe", "taken@test.com", null, null, Gender.Male, "john.doe", "Password1"),
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("User.EmailTaken");
    }

    [Fact]
    public async Task Handle_WhenUsernameTaken_ShouldReturnFailure()
    {
        _userRepository.EmailExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);
        _userRepository.UsernameExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _handler.Handle(
            new CreateUserCommand("John Doe", "john@test.com", null, null, Gender.Male, "taken_user", "Password1"),
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("User.UsernameTaken");
    }
}
