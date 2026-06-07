using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PointsTableAndExams.Infrastructure.ExternalApis.Gemini.Models;

namespace PointsTableAndExams.Infrastructure.ExternalApis.Gemini.Client;

/// <summary>
/// Responsabilidade única: executar chamadas HTTP à Gemini API.
/// Suporta dois modos de autenticação:
///   1. ServiceAccountJson → Bearer token (OAuth2) — produção
///   2. ApiKey → X-goog-api-key header — fallback desenvolvimento
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

    private GoogleCredential? _credential;
    private readonly object _credLock = new();

    private GoogleCredential GetCredential(string serviceAccountJson)
    {
        if (_credential is not null) return _credential;
        lock (_credLock)
        {
            _credential ??= GoogleCredential
                .FromJson(serviceAccountJson)
                .CreateScoped("https://www.googleapis.com/auth/generative-language");
        }
        return _credential;
    }

    public async Task<GeminiGenerateResponse> GenerateContentAsync(
        GeminiGenerateRequest request, CancellationToken ct = default)
    {
        var opts = options.Value;
        var url = opts.GenerateContentUrl;

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(request, options: JsonOptions)
        };

        if (!string.IsNullOrWhiteSpace(opts.ServiceAccountJson))
        {
            var credential = GetCredential(opts.ServiceAccountJson);
            var token = await ((ITokenAccess)credential)
                .GetAccessTokenForRequestAsync(cancellationToken: ct);
            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            logger.LogDebug("Gemini auth: service account Bearer token");
        }
        else
        {
            httpRequest.Headers.Add("X-goog-api-key", opts.ApiKey);
            logger.LogDebug("Gemini auth: API key");
        }

        logger.LogDebug("Gemini request to {Url}", url);

        var response = await http.SendAsync(httpRequest, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            logger.LogError("Gemini API error {Status}: {Body}", response.StatusCode, errorBody);
            response.EnsureSuccessStatusCode();
        }

        var json = await response.Content.ReadAsStringAsync(ct);
        logger.LogDebug("Gemini response: {Json}", json);

        return JsonSerializer.Deserialize<GeminiGenerateResponse>(json, JsonOptions)
               ?? throw new InvalidOperationException("Gemini returned an empty response.");
    }
}
