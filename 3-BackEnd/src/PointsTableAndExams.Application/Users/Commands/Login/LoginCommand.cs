using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.Users.Commands.Login;

public sealed record LoginCommand(string UsernameOrEmail, string Password) : IRequest<Result<string>>;
