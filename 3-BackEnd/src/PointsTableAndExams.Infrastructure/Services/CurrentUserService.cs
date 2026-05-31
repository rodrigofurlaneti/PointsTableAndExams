using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PointsTableAndExams.Application.Common.Interfaces;

namespace PointsTableAndExams.Infrastructure.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;

    public Guid Id => Guid.TryParse(_user?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;
    public string Username => _user?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
    public bool IsAuthenticated => _user?.Identity?.IsAuthenticated ?? false;
}
