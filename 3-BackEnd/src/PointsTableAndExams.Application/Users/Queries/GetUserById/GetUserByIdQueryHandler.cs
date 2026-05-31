using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return Result.Failure<UserResponse>(Error.NotFound);

        var response = new UserResponse(
            user.Id, user.FullName, user.Email.Value,
            user.PhoneNumber?.Value, user.BirthDate,
            user.Gender.ToString(), user.Username, user.IsActive, user.CreatedAt);

        return Result.Success(response);
    }
}
