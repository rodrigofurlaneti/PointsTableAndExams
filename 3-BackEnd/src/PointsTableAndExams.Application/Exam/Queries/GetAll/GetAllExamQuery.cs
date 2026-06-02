using MediatR;
using PointsTableAndExams.Application.Exam.DTOs;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.Exam.Queries.GetAll;

public record GetAllExamQuery() : IRequest<Result<IReadOnlyList<ExamDto>>>;
