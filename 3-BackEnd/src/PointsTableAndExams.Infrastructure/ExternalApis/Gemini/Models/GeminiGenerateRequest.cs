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
    /// <summary>True for thinking/reasoning parts returned by gemini-2.5-flash.</summary>
    public bool? Thought { get; init; }
}

public sealed record GeminiInlineData(string MimeType, string Data);

public sealed record GeminiGenerationConfig(
    double Temperature = 0.1,
    int MaxOutputTokens = 8192,
    GeminiThinkingConfig? ThinkingConfig = null);

/// <summary>
/// ThinkingBudget = 0 disables thinking for gemini-2.5-flash,
/// making it behave like a standard non-reasoning model.
/// </summary>
public sealed record GeminiThinkingConfig(int ThinkingBudget = 0);
