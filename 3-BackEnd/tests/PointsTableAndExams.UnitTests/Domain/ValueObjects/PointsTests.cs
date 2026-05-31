using FluentAssertions;
using PointsTableAndExams.Domain.Exceptions;
using PointsTableAndExams.Domain.ValueObjects;

namespace PointsTableAndExams.UnitTests.Domain.ValueObjects;

public sealed class PointsTests
{
    [Fact]
    public void Create_WithPositiveValue_ShouldSucceed()
    {
        var points = Points.Create(150);
        points.Value.Should().Be(150);
    }

    [Fact]
    public void Create_WithZero_ShouldSucceed()
    {
        var points = Points.Create(0);
        points.Value.Should().Be(0);
    }

    [Fact]
    public void Create_WithNegativeValue_ShouldThrowDomainException()
    {
        var act = () => Points.Create(-1);
        act.Should().Throw<DomainException>().WithMessage("*negative*");
    }

    [Fact]
    public void Add_ShouldSumValues()
    {
        var a = Points.Create(100);
        var b = Points.Create(50);
        a.Add(b).Value.Should().Be(150);
    }

    [Fact]
    public void Subtract_ShouldNotGoBelowZero()
    {
        var a = Points.Create(10);
        var b = Points.Create(50);
        a.Subtract(b).Value.Should().Be(0);
    }
}
