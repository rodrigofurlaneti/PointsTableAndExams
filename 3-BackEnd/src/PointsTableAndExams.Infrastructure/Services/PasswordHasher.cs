using PointsTableAndExams.Application.Common.Interfaces;

namespace PointsTableAndExams.Infrastructure.Services;

public sealed class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

    public bool Verify(string password, string hash) =>
        BCrypt.Net.BCrypt.Verify(password, hash);
}
