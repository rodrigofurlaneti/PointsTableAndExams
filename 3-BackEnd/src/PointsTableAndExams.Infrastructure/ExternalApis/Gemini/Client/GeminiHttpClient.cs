using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PointsTableAndExams.Infrastructure.ExternalApis.Gemini.Models;

namespace PointsTableAndExams.Infrastructure.ExternalApis.Gemini.Client;

/// <summary>
/// Responsabilidade única: executar chamadas HTTP à Gemini API.
/// Não conhece o domínio — retorna o contrato bruto da API.
/// </summary>
public sealed class GeminiHttpClient(
    HttpClient http,
    IOptions<GeminiHttpClientOptions> options,
    ILogger<GeminiHttpClient> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<GeminiGenerateResponse> GenerateContentAsync(
        GeminiGenerateRequest request, CancellationToken ct = default)
    {
        var opts = options.Value;
        var url = opts.GenerateContentUrl;

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(request, options: JsonOptions)
        };

        // AQ. prefix = OAuth2 access token → Authorization: Bearer
        // AIza prefix = traditional API key  → X-goog-api-key
        if (opts.ApiKey.StartsWith("AQ.", StringComparison.Ordinal))
            httpRequest.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", opts.ApiKey);
        else
            httpRequest.Headers.Add("X-goog-api-key", opts.ApiKey);

        logger.LogDebug("Gemini request to {Url}", url);

        var response = await http.SendAsync(httpRequest, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            logger.LogError("Gemini API error {Status}: {Body}", response.StatusCode, errorBody);
            response.EnsureSuccessStatusCode(); // lança HttpRequestException
        }

        var json = await response.Content.ReadAsStringAsync(ct);
        logger.LogDebug("Gemini response: {Json}", json);

        return JsonSerializer.Deserialize<GeminiGenerateResponse>(json, JsonOptions)
               ?? throw new InvalidOperationException("Gemini returned an empty response.");
    }
}
