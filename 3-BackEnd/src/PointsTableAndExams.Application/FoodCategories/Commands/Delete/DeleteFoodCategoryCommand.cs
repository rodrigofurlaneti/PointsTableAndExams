using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.FoodCategories.Commands.Delete;

public record DeleteFoodCategoryCommand(Guid Id) : IRequest<Result>;
