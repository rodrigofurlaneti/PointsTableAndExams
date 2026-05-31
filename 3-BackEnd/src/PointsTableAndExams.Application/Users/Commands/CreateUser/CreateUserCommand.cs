using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Enums;

namespace PointsTableAndExams.Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string FullName, string Email, string? PhoneNumber,
    DateOnly? BirthDate, Gender Gender, string Username, string Password
) : IRequest<Result<Guid>>;
