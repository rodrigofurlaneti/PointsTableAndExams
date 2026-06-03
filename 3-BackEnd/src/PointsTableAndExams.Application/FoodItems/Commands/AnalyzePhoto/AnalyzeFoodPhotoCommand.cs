using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.FoodItems.Commands.AnalyzePhoto;

public sealed record AnalyzeFoodPhotoCommand(
    byte[] ImageBytes,
    string MimeType)
    : IRequest<Result<AnalyzeFoodPhotoResult>>;

public sealed record AnalyzeFoodPhotoResult(
    string IdentifiedFoodName,
    decimal EstimatedPortionGrams,
    decimal CaloriesPer100g,
    bool IsConfident,
    string? Notes,
    Guid? MatchedFoodItemId,
    string? MatchedFoodItemName,
    int? MatchedFoodItemPoints,   // pontos atualizados no catálogo
    bool WasAutoCreated,
    bool WasCatalogUpdated);      // true se os pontos do catálogo foram corrigidos
