using MediatR;
using PointsTableAndExams.Application.ExamCategory.DTOs;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.ExamCategory.Queries.GetById;

public record GetExamCategoryByIdQuery(Guid Id) : IRequest<Result<ExamCategoryResponse>>;
