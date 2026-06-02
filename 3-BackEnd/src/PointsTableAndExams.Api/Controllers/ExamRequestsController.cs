using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointsTableAndExams.Application.ExamRequests.Commands.CreateExamRequest;
using PointsTableAndExams.Application.ExamRequests.Commands.MarkExamCompleted;
using PointsTableAndExams.Application.ExamRequests.Queries.GetAll;
using PointsTableAndExams.Application.ExamRequests.Queries.GetExamRequestById;

namespace PointsTableAndExams.Api.Controllers;

[Authorize]
public sealed class ExamRequestsController(IMediator mediator) : BaseApiController(mediator)
{
    /// <summary>Get all exam requests, optionally filtered by user.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? userId, CancellationToken ct) =>
        FromResult(await Mediator.Send(new GetAllExamRequestsQuery(userId), ct));

    /// <summary>Get exam request by ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct) =>
        FromResult(await Mediator.Send(new GetExamRequestByIdQuery(id), ct));

    /// <summary>Create a new exam request.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExamRequestRequest req, CancellationToken ct)
    {
        var cmd = new CreateExamRequestCommand(req.UserId, req.RequestDate, req.DoctorName, req.Notes, req.ExamIds);
        var result = await Mediator.Send(cmd, ct);
        if (result.IsFailure) return BadRequest(new { result.Error.Code, result.Error.Description });
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    /// <summary>Mark an exam as completed.</summary>
    [HttpPatch("{requestId:guid}/items/{itemId:guid}/complete")]
    public async Task<IActionResult> Complete(Guid requestId, Guid itemId,
        [FromBody] CompleteExamRequest req, CancellationToken ct)
    {
        var cmd = new MarkExamCompletedCommand(requestId, itemId, req.CompletedDate, req.Result, req.Laboratory);
        return FromResult(await Mediator.Send(cmd, ct));
    }
}

public sealed record CreateExamRequestRequest(
    Guid UserId, DateOnly RequestDate, string? DoctorName, string? Notes, IReadOnlyList<Guid> ExamIds);

public sealed record CompleteExamRequest(DateOnly CompletedDate, string? Result, string? Laboratory);
