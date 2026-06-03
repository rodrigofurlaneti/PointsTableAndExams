using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using PointsTableAndExams.Domain.Interfaces.Services;

namespace PointsTableAndExams.Application.FoodItems.Commands.AnalyzePhoto;

public sealed class AnalyzeFoodPhotoCommandHandler(
    IGeminiVisionService gemini,
    IFoodItemRepository foodItemRepository,
    IFoodCategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AnalyzeFoodPhotoCommand, Result<AnalyzeFoodPhotoResult>>
{
    // 100 kcal = 24 pontos  →  pontos = (calorias / 100) * 24
    private const decimal PointsPerHundredKcal = 24m;

    public async Task<Result<AnalyzeFoodPhotoResult>> Handle(
        AnalyzeFoodPhotoCommand request, CancellationToken cancellationToken)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

        // 1. Gemini identifica o alimento
        var analysis = await gemini.AnalyzeAsync(request.ImageBytes, request.MimeType, cts.Token);

        // 2. Busca na lista existente pelo nome
        var matches = await foodItemRepository.SearchByNameAsync(
            analysis.IdentifiedFoodName, CancellationToken.None);

        var matched = matches.FirstOrDefault();

        if (matched is not null)
        {
            var correctPoints = CalculatePoints(analysis.CaloriesPer100g, analysis.EstimatedPortionGrams);
            var wasCatalogUpdated = false;

            // Verifica se os pontos do catálogo diferem do cálculo correto
            if (analysis.CaloriesPer100g > 0 && matched.Points.Value != correctPoints)
            {
                matched.Update(
                    matched.Name,
                    $"{analysis.EstimatedPortionGrams}g",
                    correctPoints,
                    matched.Notes);

                await foodItemRepository.UpdateAsync(matched, CancellationToken.None);
                await unitOfWork.CommitAsync(CancellationToken.None);
                wasCatalogUpdated = true;
            }

            return Result.Success(new AnalyzeFoodPhotoResult(
                IdentifiedFoodName: analysis.IdentifiedFoodName,
                EstimatedPortionGrams: analysis.EstimatedPortionGrams,
                CaloriesPer100g: analysis.CaloriesPer100g,
                IsConfident: analysis.IsConfident,
                Notes: analysis.Notes,
                MatchedFoodItemId: matched.Id,
                MatchedFoodItemName: matched.Name,
                MatchedFoodItemPoints: correctPoints,
                WasAutoCreated: false,
                WasCatalogUpdated: wasCatalogUpdated));
        }

        // 3. Não encontrou → cria automaticamente usando a fórmula de pontos
        // Pontos = calorias totais da porção / 100 * 24
        var points = CalculatePoints(analysis.CaloriesPer100g, analysis.EstimatedPortionGrams);
        var category = await GetOrCreatePhotoCategoryAsync(CancellationToken.None);

        var newFoodResult = FoodItem.Create(
            categoryId: category.Id,
            name: analysis.IdentifiedFoodName,
            servingSize: $"{analysis.EstimatedPortionGrams}g",
            points: points,
            notes: $"Auto-created from photo. ~{analysis.CaloriesPer100g} kcal/100g.");

        if (newFoodResult.IsFailure)
            return Result.Failure<AnalyzeFoodPhotoResult>(newFoodResult.Error);

        var newFood = newFoodResult.Value;
        await foodItemRepository.AddAsync(newFood, CancellationToken.None);
        await unitOfWork.CommitAsync(CancellationToken.None);

        return Result.Success(new AnalyzeFoodPhotoResult(
            IdentifiedFoodName: analysis.IdentifiedFoodName,
            EstimatedPortionGrams: analysis.EstimatedPortionGrams,
            CaloriesPer100g: analysis.CaloriesPer100g,
            IsConfident: analysis.IsConfident,
            Notes: analysis.Notes,
            MatchedFoodItemId: newFood.Id,
            MatchedFoodItemName: newFood.Name,
            MatchedFoodItemPoints: newFood.Points.Value,
            WasAutoCreated: true,
            WasCatalogUpdated: false));
    }

    private static int CalculatePoints(decimal caloriesPer100g, decimal portionGrams)
    {
        if (caloriesPer100g <= 0 || portionGrams <= 0) return 1;
        var totalCalories = portionGrams / 100m * caloriesPer100g;
        return (int)Math.Round(totalCalories / 100m * PointsPerHundredKcal);
    }

    private async Task<FoodCategory> GetOrCreatePhotoCategoryAsync(CancellationToken ct)
    {
        var matches = await categoryRepository.SearchByNameAsync("Outros", ct);
        if (matches.FirstOrDefault() is { } existing) return existing;

        var result = FoodCategory.Create("Outros", "Alimentos identificados por foto", null, "g", 99);
        var category = result.Value;
        await categoryRepository.AddAsync(category, ct);
        await unitOfWork.CommitAsync(ct);
        return category;
    }
}
