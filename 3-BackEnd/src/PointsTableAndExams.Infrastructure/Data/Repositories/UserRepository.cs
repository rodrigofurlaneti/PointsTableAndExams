using Microsoft.EntityFrameworkCore;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;
using PointsTableAndExams.Infrastructure.Data;

namespace PointsTableAndExams.Infrastructure.Data.Repositories;

public sealed class UserRepository(AppDbContext context)
    : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Value == email.ToLowerInvariant(), ct);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
        await DbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username.ToLowerInvariant(), ct);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default) =>
        await DbSet.AnyAsync(u => u.Email.Value == email.ToLowerInvariant(), ct);

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken ct = default) =>
        await DbSet.AnyAsync(u => u.Username == username.ToLowerInvariant(), ct);
}
