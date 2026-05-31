using System.Text.Json;
using FluentAssertions;
using PointsTableAndExams.E2ETests.Support;

namespace PointsTableAndExams.E2ETests.Tests;

/// <summary>
/// E2E tests via Playwright API client against a running instance of the API.
/// Run the API before executing these tests: dotnet run --project src/PointsTableAndExams.Api
/// </summary>
public sealed class AuthE2ETests(PlaywrightFixture fixture) : IClassFixture<PlaywrightFixture>
{
    private readonly string _uniqueEmail = $"e2e_{Guid.NewGuid():N}@test.com";
    private readonly string _uniqueUsername = $"e2e_{Guid.NewGuid():N}"[..15];

    [Fact]
    public async Task FullAuthFlow_RegisterLoginAccessProtected_ShouldSucceed()
    {
        // 1. Register
        var registerPayload = new
        {
            FullName = "E2E Test User",
            Email = _uniqueEmail,
            Gender = 1,
            Username = _uniqueUsername,
            Password = "SecurePass1"
        };

        var registerResponse = await fixture.ApiContext.PostAsync("/api/auth/register",
            new() { DataObject = registerPayload });

        registerResponse.Status.Should().Be(201, "registration should return 201 Created");

        // 2. Login
        var loginResponse = await fixture.ApiContext.PostAsync("/api/auth/login",
            new() { DataObject = new { UsernameOrEmail = _uniqueUsername, Password = "SecurePass1" } });

        loginResponse.Status.Should().Be(200, "login should return 200 OK");

        var loginBody = JsonSerializer.Deserialize<JsonElement>(await loginResponse.TextAsync());
        var token = loginBody.GetProperty("token").GetString();
        token.Should().NotBeNullOrEmpty("a JWT token must be returned");

        // 3. Access protected endpoint
        var headers = new Dictionary<string, string> { ["Authorization"] = $"Bearer {token}" };
        var meResponse = await fixture.ApiContext.GetAsync("/api/users/" + Guid.NewGuid(),
            new() { Headers = headers });

        // 404 is fine — the user just doesn't exist yet; 401 would mean auth failed
        meResponse.Status.Should().NotBe(401, "a valid token must not be rejected");
    }

    [Fact]
    public async Task Login_WithWrongCredentials_ShouldReturn401()
    {
        var response = await fixture.ApiContext.PostAsync("/api/auth/login",
            new() { DataObject = new { UsernameOrEmail = "nobody", Password = "wrong" } });

        response.Status.Should().Be(401);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithoutToken_ShouldReturn401()
    {
        var response = await fixture.ApiContext.GetAsync("/api/daily-logs/" + Guid.NewGuid() + "/" + DateOnly.FromDateTime(DateTime.UtcNow));
        response.Status.Should().Be(401);
    }
}
