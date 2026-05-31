using System.Net;
using System.Text.Json;
using FluentAssertions;
using PointsTableAndExams.IntegrationTests.Common;

namespace PointsTableAndExams.IntegrationTests.Controllers;

public sealed class AuthControllerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Register_WithValidData_ShouldReturn201()
    {
        var payload = new
        {
            FullName = "Integration User",
            Email = $"integration_{Guid.NewGuid()}@test.com",
            Gender = 1,
            Username = $"intuser_{Guid.NewGuid():N}"[..15],
            Password = "SecurePass1"
        };

        var response = await Client.PostAsync("/api/auth/register", JsonContent(payload));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ShouldReturn400()
    {
        var email = $"dup_{Guid.NewGuid()}@test.com";
        var username1 = $"user1_{Guid.NewGuid():N}"[..15];
        var username2 = $"user2_{Guid.NewGuid():N}"[..15];

        await Client.PostAsync("/api/auth/register", JsonContent(new
        {
            FullName = "User 1", Email = email, Gender = 1, Username = username1, Password = "SecurePass1"
        }));

        var response = await Client.PostAsync("/api/auth/register", JsonContent(new
        {
            FullName = "User 2", Email = email, Gender = 1, Username = username2, Password = "SecurePass1"
        }));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        var username = $"logintest_{Guid.NewGuid():N}"[..15];
        await Client.PostAsync("/api/auth/register", JsonContent(new
        {
            FullName = "Login Test", Email = $"{username}@test.com",
            Gender = 1, Username = username, Password = "SecurePass1"
        }));

        var response = await Client.PostAsync("/api/auth/login",
            JsonContent(new { UsernameOrEmail = username, Password = "SecurePass1" }));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());
        content.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithWrongPassword_ShouldReturn401()
    {
        var response = await Client.PostAsync("/api/auth/login",
            JsonContent(new { UsernameOrEmail = "nobody", Password = "WrongPass1" }));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithoutToken_ShouldReturn401()
    {
        var response = await Client.GetAsync($"/api/users/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
