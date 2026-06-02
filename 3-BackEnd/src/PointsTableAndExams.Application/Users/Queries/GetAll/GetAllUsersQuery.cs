using MediatR;
using PointsTableAndExams.Application.Users.Queries.GetUserById;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.Users.Queries.GetAll;

public record GetAllUsersQuery() : IRequest<Result<IReadOnlyList<UserResponse>>>;
