using MediatR;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserResponse>>;

public sealed record UserResponse(
    Guid Id, string FullName, string Email,
    string? PhoneNumber, DateOnly? BirthDate,
    string Gender, string Username, bool IsActive, DateTime CreatedAt);
