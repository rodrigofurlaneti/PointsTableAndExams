using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointsTableAndExams.Application.Common.Interfaces;
using PointsTableAndExams.Application.DailyLogs.Commands.AddLogItem;
using PointsTableAndExams.Application.DailyLogs.Commands.CreateDailyLog;
using PointsTableAndExams.Application.DailyLogs.Queries.GetDailyLogByDate;
using PointsTableAndExams.Application.DailyLogs.Queries.GetHistory;

namespace PointsTableAndExams.Api.Controllers;

[Authorize]
[Route("api/daily-logs")]
public sealed class DailyLogsController(IMediator mediator, ICurrentUser currentUser)
    : BaseApiController(mediator)
{
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(
        [FromQuery] DateOnly from, [FromQuery] DateOnly to, CancellationToken ct) =>
        FromResult(await Mediator.Send(
            new GetDailyLogHistoryQuery(currentUser.Id, from, to), ct));

    [HttpGet("{date}")]
    public async Task<IActionResult> GetByDate(DateOnly date, CancellationToken ct) =>
        FromResult(await Mediator.Send(
            new GetDailyLogByDateQuery(currentUser.Id, date), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDailyLogRequest req, CancellationToken ct)
    {
        var result = await Mediator.Send(
            new CreateDailyLogCommand(currentUser.Id, req.LogDate, req.Notes), ct);
        if (result.IsFailure) return BadRequest(new { result.Error.Code, result.Error.Description });
        return CreatedAtAction(nameof(GetByDate), new { date = req.LogDate }, new { id = result.Value });
    }

    [HttpPost("{logId:guid}/items")]
    public async Task<IActionResult> AddItem(Guid logId, [FromBody] AddLogItemRequest req, CancellationToken ct)
    {
        var cmd = new AddLogItemCommand(logId, req.FoodItemId, req.Quantity, req.PointsPerServing, req.MealTime, req.Notes);
        var result = await Mediator.Send(cmd, ct);
        if (result.IsFailure) return BadRequest(new { result.Error.Code, result.Error.Description });
        return Ok(new { id = result.Value });
    }
}

public sealed record CreateDailyLogRequest(DateOnly LogDate, string? Notes);
public sealed record AddLogItemRequest(Guid FoodItemId, decimal Quantity, int PointsPerServing, TimeOnly? MealTime, string? Notes);
