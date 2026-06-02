using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<Result>;
