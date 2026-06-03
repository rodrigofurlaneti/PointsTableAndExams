using System.Text.Json;
using Microsoft.Extensions.Logging;
using PointsTableAndExams.Domain.Interfaces.Services;
using PointsTableAndExams.Infrastructure.ExternalApis.Gemini.Client;
using PointsTableAndExams.Infrastructure.ExternalApis.Gemini.Models;

namespace PointsTableAndExams.Infrastructure.Services;

/// <summary>
/// Anti-Corruption Layer (ACL) — traduz o contrato da Gemini API
/// para o modelo de domínio da aplicação.
/// Não conhece HTTP: delega ao GeminiHttpClient.
/// </summary>
public sealed class GeminiVisionService(
    GeminiHttpClient geminiClient,
    ILogger<GeminiVisionService> logger) : IGeminiVisionService
{
    private const string AnalysisPrompt = """
        Analyze this food photo and respond ONLY with valid JSON (no markdown, no explanation):
        {
          "food_name": "name of the food in English",
          "portion_grams": estimated weight in grams as a number,
          "notes": "brief note on identification and portion estimate",
          "is_confident": true or false
        }
        If you cannot identify the food, set is_confident to false and use your best guess.
        """;

    public async Task<FoodPhotoAnalysisResult> AnalyzeAsync(
        byte[] imageBytes, string mimeType, CancellationToken ct = default)
    {
        try
        {
            var request = BuildRequest(imageBytes, mimeType);
            var response = await geminiClient.GenerateContentAsync(request, ct);
            return Translate(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Gemini Vision analysis failed.");
            return new FoodPhotoAnalysisResult(
                IdentifiedFoodName: "Unknown",
                EstimatedPortionGrams: 100,
                Notes: $"Analysis failed: {ex.Message}",
                IsConfident: false);
        }
    }

    // ── Private: request builder ──────────────────────────────────────────────

    private static GeminiGenerateRequest BuildRequest(byte[] imageBytes, string mimeType) =>
        new(
            Contents:
            [
                new GeminiContent(Parts:
                [
                    new GeminiPart { Text = AnalysisPrompt },
                    new GeminiPart
                    {
                        InlineData = new GeminiInlineData(
                            MimeType: mimeType,
                            Data: Convert.ToBase64String(imageBytes))
                    }
                ])
            ],
            GenerationConfig: new GeminiGenerationConfig(Temperature: 0.1, MaxOutputTokens: 1024)
        );

    // ── Private: ACL translation ──────────────────────────────────────────────

    private FoodPhotoAnalysisResult Translate(GeminiGenerateResponse response)
    {
        var rawText = response.Candidates
            .FirstOrDefault()
            ?.Content.Parts
            .FirstOrDefault()
            ?.Text ?? "{}";

        var text = StripMarkdownFences(rawText);

        logger.LogInformation("Gemini translated text: {Text}", text);

        var json = JsonDocument.Parse(text).RootElement;

        return new FoodPhotoAnalysisResult(
            IdentifiedFoodName: json.GetProperty("food_name").GetString() ?? "Unknown",
            EstimatedPortionGrams: json.GetProperty("portion_grams").GetDecimal(),
            Notes: json.TryGetProperty("notes", out var n) ? n.GetString() : null,
            IsConfident: json.TryGetProperty("is_confident", out var c) && c.GetBoolean());
    }

    private static string StripMarkdownFences(string text)
    {
        text = text.Trim();
        if (text.StartsWith("```"))
            text = text[(text.IndexOf('\n') + 1)..];
        if (text.EndsWith("```"))
            text = text[..text.LastIndexOf("```")];
        return text.Trim();
    }
}
