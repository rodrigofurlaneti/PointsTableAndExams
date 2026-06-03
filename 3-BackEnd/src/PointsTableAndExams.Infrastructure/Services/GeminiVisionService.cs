using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PointsTableAndExams.Domain.Interfaces.Services;

namespace PointsTableAndExams.Infrastructure.Services;

public sealed class GeminiVisionService(
    HttpClient http,
    IConfiguration config,
    ILogger<GeminiVisionService> logger) : IGeminiVisionService
{
    private const string Prompt = """
        Analyze this food photo and respond ONLY with valid JSON (no markdown):
        {
          "food_name": "name of the food in English",
          "portion_grams": estimated weight in grams as a number,
          "notes": "brief note on identification and portion estimate",
          "is_confident": true or false
        }
        If you cannot identify the food, set is_confident to false.
        """;

    public async Task<FoodPhotoAnalysisResult> AnalyzeAsync(
        byte[] imageBytes, string mimeType, CancellationToken ct = default)
    {
        var apiKey = config["Gemini:ApiKey"]
            ?? throw new InvalidOperationException("Gemini:ApiKey not configured.");

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new { text = Prompt },
                        new { inline_data = new { mime_type = mimeType, data = Convert.ToBase64String(imageBytes) } }
                    }
                }
            },
            generationConfig = new { temperature = 0.1, maxOutputTokens = 1024 }
        };

        const string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent";

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("X-goog-api-key", apiKey);
            request.Content = JsonContent.Create(requestBody);
            var response = await http.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                logger.LogError("Gemini API error {Status}: {Body}", response.StatusCode, errorBody);
                response.EnsureSuccessStatusCode();
            }

            var json = await response.Content.ReadAsStringAsync(ct);
            var doc = JsonDocument.Parse(json);

            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "{}";

            // Remove markdown code fences if Gemini adds them
            text = text.Trim();
            if (text.StartsWith("```")) text = text[(text.IndexOf('\n') + 1)..];
            if (text.EndsWith("```")) text = text[..text.LastIndexOf("```")];
            text = text.Trim();

            logger.LogInformation("Gemini raw text: {Text}", text);

            var result = JsonDocument.Parse(text).RootElement;

            return new FoodPhotoAnalysisResult(
                IdentifiedFoodName: result.GetProperty("food_name").GetString() ?? "Unknown",
                EstimatedPortionGrams: result.GetProperty("portion_grams").GetDecimal(),
                Notes: result.TryGetProperty("notes", out var n) ? n.GetString() : null,
                IsConfident: result.TryGetProperty("is_confident", out var c) && c.GetBoolean());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Gemini Vision analysis failed.");
            return new FoodPhotoAnalysisResult("Unknown", 100, $"Analysis failed: {ex.Message}", false);
        }
    }
}
