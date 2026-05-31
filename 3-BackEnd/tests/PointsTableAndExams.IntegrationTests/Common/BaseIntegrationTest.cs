using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PointsTableAndExams.IntegrationTests.Common;

public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient Client = factory.CreateClient();
    protected readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    protected StringContent JsonContent<T>(T obj) =>
        new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

    protected async Task AuthenticateAsync(string username = "testuser", string password = "TestPass1")
    {
        // Register
        await Client.PostAsync("/api/auth/register", JsonContent(new
        {
            FullName = "Test User",
            Email = $"{username}@test.com",
            Gender = 1,
            Username = username,
            Password = password
        }));

        // Login
        var response = await Client.PostAsync("/api/auth/login",
            JsonContent(new { UsernameOrEmail = username, Password = password }));

        var content = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<JsonElement>(content).GetProperty("token").GetString();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token!);
    }
}
