using FluentAssertions;
using NetArchTest.Rules;

// Explicit assembly markers using global aliases
using DomainEntity        = PointsTableAndExams.Domain.Common.Entity;
using AppDI               = PointsTableAndExams.Application.DependencyInjection;

namespace PointsTableAndExams.UnitTests.Architecture;

public sealed class ArchitectureTests
{
    private const string ApplicationNs    = "PointsTableAndExams.Application";
    private const string InfrastructureNs = "PointsTableAndExams.Infrastructure";
    private const string ApiNs            = "PointsTableAndExams.Api";

    [Fact]
    public void Domain_ShouldNotDependOn_Application()
    {
        Types.InAssembly(typeof(DomainEntity).Assembly)
            .ShouldNot().HaveDependencyOn(ApplicationNs)
            .GetResult().IsSuccessful.Should().BeTrue("Domain must not depend on Application.");
    }

    [Fact]
    public void Domain_ShouldNotDependOn_Infrastructure()
    {
        Types.InAssembly(typeof(DomainEntity).Assembly)
            .ShouldNot().HaveDependencyOn(InfrastructureNs)
            .GetResult().IsSuccessful.Should().BeTrue("Domain must not depend on Infrastructure.");
    }

    [Fact]
    public void Application_ShouldNotDependOn_Infrastructure()
    {
        Types.InAssembly(typeof(AppDI).Assembly)
            .ShouldNot().HaveDependencyOn(InfrastructureNs)
            .GetResult().IsSuccessful.Should().BeTrue("Application must not depend on Infrastructure.");
    }

    [Fact]
    public void Application_ShouldNotDependOn_Api()
    {
        Types.InAssembly(typeof(AppDI).Assembly)
            .ShouldNot().HaveDependencyOn(ApiNs)
            .GetResult().IsSuccessful.Should().BeTrue("Application must not depend on Api.");
    }

    [Fact]
    public void Domain_Entities_ShouldHavePrivateSetters()
    {
        var entities = Types.InAssembly(typeof(DomainEntity).Assembly)
            .That().Inherit(typeof(DomainEntity))
            .GetTypes();

        foreach (var entity in entities)
        {
            var publicSetters = entity.GetProperties()
                .Where(p => p.CanWrite && p.SetMethod?.IsPublic == true)
                .ToList();

            publicSetters.Should().BeEmpty(
                because: $"Entity '{entity.Name}' should not have public setters (Object Calisthenics).");
        }
    }

    [Fact]
    public void Handlers_ShouldBeSealed()
    {
        Types.InAssembly(typeof(AppDI).Assembly)
            .That().HaveNameEndingWith("Handler")
            .Should().BeSealed()
            .GetResult().IsSuccessful.Should().BeTrue("All command/query handlers must be sealed.");
    }
}
