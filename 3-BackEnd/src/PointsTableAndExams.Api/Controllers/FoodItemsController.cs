using MediatR;
using Microsoft.AspNetCore.Mvc;
using PointsTableAndExams.Application.FoodItems.Commands.Create;
using PointsTableAndExams.Application.FoodItems.Commands.Delete;
using PointsTableAndExams.Application.FoodItems.Commands.Update;
using PointsTableAndExams.Application.FoodItems.Queries.GetAll;
using PointsTableAndExams.Application.FoodItems.Queries.GetById;

namespace PointsTableAndExams.Api.Controllers;

[ApiController]
[Route("api/food-items")]
public class FoodItemsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllFoodItemsQuery();
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetFoodItemByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFoodItemCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, command);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFoodItemCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("O ID da rota difere do ID do corpo da requisição.");

        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteFoodItemCommand(id);
        var result = await mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return NoContent();
    }
}