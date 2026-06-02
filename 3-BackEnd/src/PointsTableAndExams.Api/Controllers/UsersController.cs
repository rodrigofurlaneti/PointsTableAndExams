using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointsTableAndExams.Application.Users.Commands.DeleteUser;
using PointsTableAndExams.Application.Users.Commands.UpdateUser;
using PointsTableAndExams.Application.Users.Queries.GetAll;
using PointsTableAndExams.Application.Users.Queries.GetUserById;

namespace PointsTableAndExams.Api.Controllers;

[Authorize]
public sealed class UsersController(IMediator mediator) : BaseApiController(mediator)
{
    /// <summary>Get all users.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        FromResult(await Mediator.Send(new GetAllUsersQuery(), ct));

    /// <summary>Get user by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct) =>
        FromResult(await Mediator.Send(new GetUserByIdQuery(id), ct));

    /// <summary>Update user profile.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest req, CancellationToken ct)
    {
        if (id != req.Id)
            return BadRequest("O ID da rota difere do ID do corpo da requisição.");

        return FromResult(await Mediator.Send(new UpdateUserCommand(req.Id, req.FullName, req.PhoneNumber, req.BirthDate), ct));
    }

    /// <summary>Delete user.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct) =>
        FromResult(await Mediator.Send(new DeleteUserCommand(id), ct));
}

public sealed record UpdateUserRequest(Guid Id, string FullName, string? PhoneNumber, DateOnly? BirthDate);
