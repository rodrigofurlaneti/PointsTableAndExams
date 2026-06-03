namespace PointsTableAndExams.Infrastructure.ExternalApis.Gemini.Models;

/// <summary>
/// Espelho do contrato de request da Gemini GenerateContent API.
/// Ref: https://ai.google.dev/api/generate-content
/// </summary>
public sealed record GeminiGenerateRequest(
    GeminiContent[] Contents,
    GeminiGenerationConfig? GenerationConfig = null);

public sealed record GeminiContent(GeminiPart[] Parts, string Role = "user");

public sealed record GeminiPart
{
    public string? Text { get; init; }
    public GeminiInlineData? InlineData { get; init; }
}

public sealed record GeminiInlineData(string MimeType, string Data);

public sealed record GeminiGenerationConfig(
    double Temperature = 0.1,
    int MaxOutputTokens = 1024);
