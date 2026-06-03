namespace PointsTableAndExams.Domain.Interfaces.Services;

public interface IGeminiVisionService
{
    Task<FoodPhotoAnalysisResult> AnalyzeAsync(byte[] imageBytes, string mimeType, CancellationToken ct = default);
}

public sealed record FoodPhotoAnalysisResult(
    string IdentifiedFoodName,
    decimal EstimatedPortionGrams,
    decimal CaloriesPer100g,
    string? Notes,
    bool IsConfident);
