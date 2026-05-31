using FluentAssertions;
using PointsTableAndExams.Domain.Exceptions;
using PointsTableAndExams.Domain.ValueObjects;

namespace PointsTableAndExams.UnitTests.Domain.ValueObjects;

public sealed class EmailTests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("USER@EXAMPLE.COM")]
    [InlineData("user.name+tag@domain.co")]
    public void Create_WithValidEmail_ShouldSucceed(string email)
    {
        var result = Email.Create(email);
        result.Value.Should().Be(email.ToLowerInvariant());
    }

    [Theory]
    [InlineData("")]
    [InlineData("notanemail")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    public void Create_WithInvalidEmail_ShouldThrowDomainException(string email)
    {
        var act = () => Email.Create(email);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void TwoEmailsWithSameValue_ShouldBeEqual()
    {
        var a = Email.Create("user@example.com");
        var b = Email.Create("USER@example.com");
        a.Should().Be(b);
    }
}
