using FluentAssertions;
using PointsTableAndExams.Domain.DomainEvents;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Enums;
using PointsTableAndExams.Domain.Exceptions;

namespace PointsTableAndExams.UnitTests.Domain.Entities;

public sealed class UserTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateUser()
    {
        var user = User.Create("John Doe", "john@test.com", "11999990001",
            new DateOnly(1990, 1, 1), Gender.Male, "john.doe", "hashedPassword");

        user.FullName.Should().Be("John Doe");
        user.Email.Value.Should().Be("john@test.com");
        user.Username.Should().Be("john.doe");
        user.IsActive.Should().BeTrue();
        user.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_ShouldRaiseUserCreatedDomainEvent()
    {
        var user = User.Create("Jane Doe", "jane@test.com", null,
            null, Gender.Female, "jane.doe", "hash");

        user.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<UserCreatedEvent>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null!)]
    public void Create_WithEmptyFullName_ShouldThrowDomainException(string? fullName)
    {
        var act = () => User.Create(fullName!, "john@test.com", null,
            null, Gender.Male, "john.doe", "hash");

        act.Should().Throw<DomainException>()
            .WithMessage("*Full name*");
    }

    [Fact]
    public void UpdateProfile_ShouldUpdateFields()
    {
        var user = User.Create("John Doe", "john@test.com", null,
            null, Gender.Male, "john.doe", "hash");

        user.UpdateProfile("John Updated", "11888880001", new DateOnly(1990, 5, 20));

        user.FullName.Should().Be("John Updated");
        user.PhoneNumber!.Value.Should().Be("11888880001");
        user.BirthDate.Should().Be(new DateOnly(1990, 5, 20));
        user.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        var user = User.Create("John Doe", "john@test.com", null,
            null, Gender.Male, "john.doe", "hash");

        user.Deactivate();

        user.IsActive.Should().BeFalse();
    }
}
