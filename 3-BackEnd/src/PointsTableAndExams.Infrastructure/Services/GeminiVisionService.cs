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
        Analyze this food photo. Respond ONLY with a JSON object — no markdown, no explanation, just raw JSON.
        Use exactly this structure:
        {
          "food_name": "English name of the food",
          "portion_grams": 150,
          "calories_per_100g": 250,
          "notes": "Brief description of how you identified it and estimated the portion",
          "is_confident": true
        }
        Rules:
        - portion_grams: integer, estimated total weight of the food shown
        - calories_per_100g: integer, average calories per 100g for this food type (required, never omit)
        - is_confident: false if you cannot identify the food with reasonable certainty
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
                CaloriesPer100g: 0,
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
            GenerationConfig: new GeminiGenerationConfig(
                Temperature: 0.1,
                MaxOutputTokens: 8192,
                ThinkingConfig: new GeminiThinkingConfig(ThinkingBudget: 0))
        );

    // ── Private: ACL translation ──────────────────────────────────────────────

    private FoodPhotoAnalysisResult Translate(GeminiGenerateResponse response)
    {
        var rawText = response.Candidates
            .FirstOrDefault()
            ?.Content.Parts
            .FirstOrDefault(p => p.Thought != true && p.Text is not null)
            ?.Text ?? "{}";

        var text = StripMarkdownFences(rawText);

        logger.LogInformation("Gemini translated text: {Text}", text);

        var json = JsonDocument.Parse(text).RootElement;

        return new FoodPhotoAnalysisResult(
            IdentifiedFoodName: json.GetProperty("food_name").GetString() ?? "Unknown",
            EstimatedPortionGrams: json.GetProperty("portion_grams").GetDecimal(),
            CaloriesPer100g: json.TryGetProperty("calories_per_100g", out var cal) ? cal.GetDecimal() : 0,
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
