using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string FullName,
    string? PhoneNumber,
    DateOnly? BirthDate) : IRequest<Result>;
