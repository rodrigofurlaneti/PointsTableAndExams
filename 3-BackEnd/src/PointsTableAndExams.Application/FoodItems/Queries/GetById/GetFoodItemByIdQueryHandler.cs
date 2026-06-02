using MediatR;
using PointsTableAndExams.Application.FoodItems.DTOs;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointsTableAndExams.Application.FoodItems.Queries.GetById
{
    public sealed class GetFoodItemByIdQueryHandler(IFoodItemRepository repository)
    : IRequestHandler<GetFoodItemByIdQuery, Result<FoodItemResponse>>
    {
        public async Task<Result<FoodItemResponse>> Handle(GetFoodItemByIdQuery request, CancellationToken cancellationToken)
        {
            var foodItem = await repository.GetByIdAsync(request.Id, cancellationToken);

            // Fail fast: sem o uso de "else", seguindo Object Calisthenics
            if (foodItem is null)
                return Result.Failure<FoodItemResponse>(new Error("NotFound", "Alimento não encontrado."));

            // Mapeamento corrigido para os nomes corretos definidos na sua modelagem
            var response = new FoodItemResponse(
                foodItem.Id,
                foodItem.Name,
                foodItem.Points.Value, // Assumindo Value Object para Points
                foodItem.ServingSize,  // Corrigido de Portion para ServingSize
                foodItem.Notes,        // Adicionado Notes
                foodItem.FoodCategoryId, // Corrigido de CategoryId para FoodCategoryId
                foodItem.IsActive);

            return Result.Success<FoodItemResponse>(response);
        }
    }
}
