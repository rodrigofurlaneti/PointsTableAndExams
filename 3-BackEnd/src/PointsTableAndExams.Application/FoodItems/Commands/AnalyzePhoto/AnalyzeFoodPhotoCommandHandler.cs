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
        // 1. Gemini identifica o alimento na foto
        var analysis = await gemini.AnalyzeAsync(
            request.ImageBytes, request.MimeType, cancellationToken);

        // 2. Busca o alimento na lista existente pelo nome
        var matches = await foodItemRepository.SearchByNameAsync(
            analysis.IdentifiedFoodName, cancellationToken);

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
