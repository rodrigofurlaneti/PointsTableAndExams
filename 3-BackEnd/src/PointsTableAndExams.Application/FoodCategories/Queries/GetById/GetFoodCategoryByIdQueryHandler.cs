using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using PointsTableAndExams.Application.FoodCategories.DTOs;

namespace PointsTableAndExams.Application.FoodCategories.Queries.GetById
{
    public class GetFoodCategoryByIdQueryHandler(IFoodCategoryRepository repository)
        : IRequestHandler<GetFoodCategoryByIdQuery, Result<FoodCategoryResponse>>
    {
        public async Task<Result<FoodCategoryResponse>> Handle(GetFoodCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Busca a entidade no banco de dados (o repositório base já usa AsNoTracking por padrão)
            var category = await repository.GetByIdAsync(request.Id, cancellationToken);

            // 2. Fail-fast: Se não encontrar, retorna falha no Result
            if (category is null)
                return Result<FoodCategoryResponse>.Failure("Categoria de alimento não encontrada.");

            // 3. Mapeia a entidade para o DTO de resposta
            var response = new FoodCategoryResponse(
                category.Id,
                category.Name,
                category.Description,
                category.DefaultQuotaPoints,
                category.ServingUnit,
                category.SortOrder,
                category.IsActive);

            // 4. Retorna sucesso com os dados
            return Result<FoodCategoryResponse>.Success(response);
        }
    }
}
