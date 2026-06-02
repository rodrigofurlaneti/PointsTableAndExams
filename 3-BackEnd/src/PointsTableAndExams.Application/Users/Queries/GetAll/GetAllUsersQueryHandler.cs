using MediatR;
using PointsTableAndExams.Application.Users.Queries.GetUserById;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.Users.Queries.GetAll;

public sealed class GetAllUsersQueryHandler(IUserRepository repository)
    : IRequestHandler<GetAllUsersQuery, Result<IReadOnlyList<UserResponse>>>
{
    public async Task<Result<IReadOnlyList<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await repository.GetAllAsync(cancellationToken);

        var dtos = users
            .Select(u => new UserResponse(
                u.Id, u.FullName, u.Email.Value,
                u.PhoneNumber?.Value, u.BirthDate,
                u.Gender.ToString(), u.Username, u.IsActive, u.CreatedAt))
            .ToList();

        return Result.Success<IReadOnlyList<UserResponse>>(dtos);
    }
}
