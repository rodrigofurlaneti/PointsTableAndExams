using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using PointsTableAndExams.Domain.Interfaces.Services;

namespace PointsTableAndExams.Application.FoodItems.Commands.AnalyzePhoto;

public sealed class AnalyzeFoodPhotoCommandHandler(
    IGeminiVisionService gemini,
    IFoodItemRepository foodItemRepository)
    : IRequestHandler<AnalyzeFoodPhotoCommand, Result<AnalyzeFoodPhotoResult>>
{
    public async Task<Result<AnalyzeFoodPhotoResult>> Handle(
        AnalyzeFoodPhotoCommand request, CancellationToken cancellationToken)
    {
        // Gemini pode demorar 10-15s — usa CancellationToken.None para não
        // ser cancelado pelo timeout padrão do ASP.NET (30s de pipeline)
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

        // 1. Gemini identifica o alimento na foto
        var analysis = await gemini.AnalyzeAsync(
            request.ImageBytes, request.MimeType, cts.Token);

        // 2. Busca o alimento na lista existente pelo nome
        // Usa CancellationToken.None pois o token original pode ter expirado
        // durante a chamada ao Gemini (que pode demorar ~4s)
        var matches = await foodItemRepository.SearchByNameAsync(
            analysis.IdentifiedFoodName, CancellationToken.None);

        var best = matches.FirstOrDefault();

        return Result.Success(new AnalyzeFoodPhotoResult(
            IdentifiedFoodName: analysis.IdentifiedFoodName,
            EstimatedPortionGrams: analysis.EstimatedPortionGrams,
            IsConfident: analysis.IsConfident,
            Notes: analysis.Notes,
            MatchedFoodItemId: best?.Id,
            MatchedFoodItemName: best?.Name,
            MatchedFoodItemPoints: best?.Points.Value));
    }
}
