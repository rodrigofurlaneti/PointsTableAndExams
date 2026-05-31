using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointsTableAndExams.Application.Users.Queries.GetUserById;

namespace PointsTableAndExams.Api.Controllers;

[Authorize]
public sealed class UsersController(IMediator mediator) : BaseApiController(mediator)
{
    /// <summary>Get user by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct) =>
        FromResult(await Mediator.Send(new GetUserByIdQuery(id), ct));
}
