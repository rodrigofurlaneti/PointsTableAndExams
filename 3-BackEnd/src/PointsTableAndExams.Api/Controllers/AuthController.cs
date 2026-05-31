using MediatR;
using Microsoft.AspNetCore.Mvc;
using PointsTableAndExams.Application.Users.Commands.CreateUser;
using PointsTableAndExams.Application.Users.Commands.Login;
using PointsTableAndExams.Domain.Enums;

namespace PointsTableAndExams.Api.Controllers;

public sealed class AuthController(IMediator mediator) : BaseApiController(mediator)
{
    /// <summary>Register a new user.</summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var command = new CreateUserCommand(
            request.FullName, request.Email, request.PhoneNumber,
            request.BirthDate, request.Gender, request.Username, request.Password);

        var result = await Mediator.Send(command, ct);
        if (result.IsFailure) return BadRequest(new { result.Error.Code, result.Error.Description });
        return CreatedAtAction(nameof(UsersController.GetById), "Users", new { id = result.Value }, null);
    }

    /// <summary>Login and obtain JWT token.</summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await Mediator.Send(new LoginCommand(request.UsernameOrEmail, request.Password), ct);
        if (result.IsFailure) return Unauthorized(new { result.Error.Code, result.Error.Description });
        return Ok(new { token = result.Value });
    }
}

public sealed record RegisterRequest(
    string FullName, string Email, string? PhoneNumber,
    DateOnly? BirthDate, Gender Gender, string Username, string Password);

public sealed record LoginRequest(string UsernameOrEmail, string Password);
