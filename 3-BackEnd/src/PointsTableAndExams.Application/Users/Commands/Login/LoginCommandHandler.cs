using MediatR;
using PointsTableAndExams.Application.Common.Interfaces;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.Users.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
    : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(request.UsernameOrEmail, cancellationToken)
            ?? await userRepository.GetByEmailAsync(request.UsernameOrEmail, cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Failure<string>(new Error("Auth.InvalidCredentials", "Invalid username or password."));

        if (!user.IsActive)
            return Result.Failure<string>(new Error("Auth.AccountDisabled", "Account is disabled."));

        var token = tokenService.GenerateToken(user);
        return Result.Success(token);
    }
}
