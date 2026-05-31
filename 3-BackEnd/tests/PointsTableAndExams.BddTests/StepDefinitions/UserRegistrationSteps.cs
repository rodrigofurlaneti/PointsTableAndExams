using FluentAssertions;
using NSubstitute;
using PointsTableAndExams.Application.Common.Interfaces;
using PointsTableAndExams.Application.Users.Commands.CreateUser;
using PointsTableAndExams.Domain.Enums;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using Reqnroll;

namespace PointsTableAndExams.BddTests.StepDefinitions;

[Binding]
public sealed class UserRegistrationSteps
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();

    private Domain.Common.Result<Guid>? _result;
    private Exception? _exception;

    [BeforeScenario]
    public void Setup() =>
        _passwordHasher.Hash(Arg.Any<string>()).Returns("hashed");

    [Given("I have valid registration data")]
    public void GivenIHaveValidRegistrationData()
    {
        _userRepository.EmailExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);
        _userRepository.UsernameExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);
    }

    [Given("a user already exists with email {string}")]
    public void GivenAUserAlreadyExistsWithEmail(string email) =>
        _userRepository.EmailExistsAsync(email, Arg.Any<CancellationToken>()).Returns(true);

    [When("I register with email {string} and username {string}")]
    public async Task WhenIRegisterWithEmailAndUsername(string email, string username)
    {
        var handler = new CreateUserCommandHandler(_userRepository, _unitOfWork, _passwordHasher);
        _result = await handler.Handle(
            new CreateUserCommand("Test User", email, null, null, Gender.Male, username, "Password1"),
            CancellationToken.None);
    }

    [When("I try to register with the same email {string}")]
    public async Task WhenITryToRegisterWithTheSameEmail(string email)
    {
        var handler = new CreateUserCommandHandler(_userRepository, _unitOfWork, _passwordHasher);
        _result = await handler.Handle(
            new CreateUserCommand("Test User", email, null, null, Gender.Male, "newuser", "Password1"),
            CancellationToken.None);
    }

    [When("I register with a weak password {string}")]
    public async Task WhenIRegisterWithAWeakPassword(string password)
    {
        try
        {
            var validator = new CreateUserCommandValidator();
            var cmd = new CreateUserCommand("Test User", "test@test.com", null, null, Gender.Male, "testuser", password);
            var validationResult = await validator.ValidateAsync(cmd);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);
        }
        catch (Exception ex)
        {
            _exception = ex;
        }
    }

    [Then("the registration should succeed")]
    public void ThenTheRegistrationShouldSucceed() =>
        _result!.IsSuccess.Should().BeTrue();

    [Then("a new user ID should be returned")]
    public void ThenANewUserIdShouldBeReturned() =>
        _result!.Value.Should().NotBeEmpty();

    [Then("the registration should fail")]
    public void ThenTheRegistrationShouldFail() =>
        _result!.IsFailure.Should().BeTrue();

    [Then("the error code should be {string}")]
    public void ThenTheErrorCodeShouldBe(string code) =>
        _result!.Error.Code.Should().Be(code);

    [Then("a validation error should occur")]
    public void ThenAValidationErrorShouldOccur() =>
        _exception.Should().BeOfType<FluentValidation.ValidationException>();
}
