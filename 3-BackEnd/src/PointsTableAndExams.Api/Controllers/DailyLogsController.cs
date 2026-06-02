using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointsTableAndExams.Application.DailyLogs.Commands.AddLogItem;
using PointsTableAndExams.Application.DailyLogs.Commands.CreateDailyLog;
using PointsTableAndExams.Application.DailyLogs.Queries.GetDailyLogByDate;
using PointsTableAndExams.Application.DailyLogs.Queries.GetHistory;

namespace PointsTableAndExams.Api.Controllers;

[Authorize]
public sealed class DailyLogsController(IMediator mediator) : BaseApiController(mediator)
{
    /// <summary>Get daily log history for a user within a date range.</summary>
    [HttpGet("{userId:guid}/history")]
    public async Task<IActionResult> GetHistory(Guid userId, [FromQuery] DateOnly from, [FromQuery] DateOnly to, CancellationToken ct) =>
        FromResult(await Mediator.Send(new GetDailyLogHistoryQuery(userId, from, to), ct));

    /// <summary>Get daily log by user and date.</summary>
    [HttpGet("{userId:guid}/{date}")]
    public async Task<IActionResult> GetByDate(Guid userId, DateOnly date, CancellationToken ct) =>
        FromResult(await Mediator.Send(new GetDailyLogByDateQuery(userId, date), ct));

    /// <summary>Create a new daily log.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDailyLogRequest req, CancellationToken ct)
    {
        var result = await Mediator.Send(new CreateDailyLogCommand(req.UserId, req.LogDate, req.Notes), ct);
        if (result.IsFailure) return BadRequest(new { result.Error.Code, result.Error.Description });
        return CreatedAtAction(nameof(GetByDate), new { userId = req.UserId, date = req.LogDate }, new { id = result.Value });
    }

    /// <summary>Add a food item to an existing daily log.</summary>
    [HttpPost("{logId:guid}/items")]
    public async Task<IActionResult> AddItem(Guid logId, [FromBody] AddLogItemRequest req, CancellationToken ct)
    {
        var cmd = new AddLogItemCommand(logId, req.FoodItemId, req.Quantity, req.PointsPerServing, req.MealTime, req.Notes);
        var result = await Mediator.Send(cmd, ct);
        if (result.IsFailure) return BadRequest(new { result.Error.Code, result.Error.Description });
        return Ok(new { id = result.Value });
    }
}

public sealed record CreateDailyLogRequest(Guid UserId, DateOnly LogDate, string? Notes);
public sealed record AddLogItemRequest(Guid FoodItemId, decimal Quantity, int PointsPerServing, TimeOnly? MealTime, string? Notes);
