namespace PointsTableAndExams.Infrastructure.ExternalApis.Gemini.Client;

public sealed class GeminiHttpClientOptions
{
    public const string SectionName = "Gemini";

    public string ApiKey { get; init; } = string.Empty;

    /// <summary>
    /// Service account JSON key content (full JSON string).
    /// When set, uses OAuth2 Bearer token auth instead of ApiKey.
    /// </summary>
    public string ServiceAccountJson { get; init; } = string.Empty;

    public string BaseUrl { get; init; } = "https://generativelanguage.googleapis.com";
    public string ApiVersion { get; init; } = "v1beta";
    public string Model { get; init; } = "gemini-2.5-flash";

    public string GenerateContentUrl =>
        $"{BaseUrl}/{ApiVersion}/models/{Model}:generateContent";
}
