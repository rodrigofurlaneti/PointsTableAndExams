namespace PointsTableAndExams.Infrastructure.ExternalApis.Gemini.Models;

/// <summary>
/// Espelho do contrato de response da Gemini GenerateContent API.
/// </summary>
public sealed record GeminiGenerateResponse(GeminiCandidate[] Candidates);

public sealed record GeminiCandidate(GeminiContent Content, string FinishReason);

public sealed record GeminiUsageMetadata(int PromptTokenCount, int CandidatesTokenCount);
