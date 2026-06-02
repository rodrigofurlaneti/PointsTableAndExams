using MediatR;
using PointsTableAndExams.Application.Exam.DTOs;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.Exam.Queries.GetById;

public record GetExamByIdQuery(Guid Id) : IRequest<Result<ExamResponse>>;
