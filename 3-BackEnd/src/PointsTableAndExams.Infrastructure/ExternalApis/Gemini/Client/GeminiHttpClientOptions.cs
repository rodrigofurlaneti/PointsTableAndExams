namespace PointsTableAndExams.Infrastructure.ExternalApis.Gemini.Client;

public sealed class GeminiHttpClientOptions
{
    public const string SectionName = "Gemini";

    public string ApiKey { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = "https://generativelanguage.googleapis.com";
    public string ApiVersion { get; init; } = "v1beta";
    public string Model { get; init; } = "gemini-flash-latest";

    public string GenerateContentUrl =>
        $"{BaseUrl}/{ApiVersion}/models/{Model}:generateContent";
}
