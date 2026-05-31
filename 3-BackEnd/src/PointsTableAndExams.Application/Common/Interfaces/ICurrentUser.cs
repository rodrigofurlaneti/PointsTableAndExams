namespace PointsTableAndExams.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid Id { get; }
    string Username { get; }
    bool IsAuthenticated { get; }
}
