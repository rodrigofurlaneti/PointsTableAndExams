using MediatR;
using PointsTableAndExams.Application.Common.Interfaces;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher)
    : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.EmailExistsAsync(request.Email, cancellationToken))
            return Result.Failure<Guid>(new Error("User.EmailTaken", $"Email '{request.Email}' is already in use."));

        if (await userRepository.UsernameExistsAsync(request.Username, cancellationToken))
            return Result.Failure<Guid>(new Error("User.UsernameTaken", $"Username '{request.Username}' is already taken."));

        var hash = passwordHasher.Hash(request.Password);
        var user = User.Create(request.FullName, request.Email, request.PhoneNumber,
            request.BirthDate, request.Gender, request.Username, hash);

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(user.Id);
    }
}
