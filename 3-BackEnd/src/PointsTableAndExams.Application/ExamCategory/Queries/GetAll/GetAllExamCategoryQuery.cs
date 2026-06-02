using MediatR;
using PointsTableAndExams.Application.ExamCategory.DTOs;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.ExamCategory.Queries.GetAll;

public record GetAllExamCategoryQuery() : IRequest<Result<IReadOnlyList<ExamCategoryDto>>>;
