using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
