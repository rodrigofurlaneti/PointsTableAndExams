using MediatR;
using Microsoft.AspNetCore.Mvc;
using PointsTableAndExams.Application.ExamCategory.Commands.Create;
using PointsTableAndExams.Application.ExamCategory.Commands.Delete;
using PointsTableAndExams.Application.ExamCategory.Commands.Update;
using PointsTableAndExams.Application.ExamCategory.Queries.GetAll;
using PointsTableAndExams.Application.ExamCategory.Queries.GetById;

namespace PointsTableAndExams.Api.Controllers;

[ApiController]
[Route("api/exam-categories")]
public sealed class ExamCategoryController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllExamCategoryQuery(), cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetExamCategoryByIdQuery(id), cancellationToken);
        if (!result.IsSuccess)
            return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExamCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetById), new { id = result.Value }, command);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExamCategoryCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("O ID da rota difere do ID do corpo da requisicao.");
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteExamCategoryCommand(id), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);
        return NoContent();
    }
}
