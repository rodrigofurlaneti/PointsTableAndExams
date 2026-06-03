using MediatR;
using Microsoft.AspNetCore.Mvc;
using PointsTableAndExams.Application.FoodItems.Commands.AnalyzePhoto;
using PointsTableAndExams.Application.FoodItems.Commands.Create;
using PointsTableAndExams.Application.FoodItems.Commands.Delete;
using PointsTableAndExams.Application.FoodItems.Commands.Update;
using PointsTableAndExams.Application.FoodItems.Queries.GetAll;
using PointsTableAndExams.Application.FoodItems.Queries.GetById;

namespace PointsTableAndExams.Api.Controllers;

[ApiController]
[Route("api/food-items")]
public sealed class FoodItemsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllFoodItemsQuery(), cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetFoodItemByIdQuery(id), cancellationToken);
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
            return BadRequest("O ID da rota difere do ID do corpo da requisicao.");
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteFoodItemCommand(id), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.Error);
        return NoContent();
    }

    /// <summary>
    /// Envia uma foto de um prato. O Gemini identifica o alimento e busca
    /// na lista existente. Retorna o item correspondente (se encontrado)
    /// com os Points para o usuário confirmar antes de registrar no log.
    /// </summary>
    [HttpPost("analyze-photo")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AnalyzePhoto(
        IFormFile photo, CancellationToken cancellationToken)
    {
        if (photo is null || photo.Length == 0)
            return BadRequest(new { Code = "Validation.Required", Description = "No photo uploaded." });

        using var ms = new MemoryStream();
        await photo.CopyToAsync(ms, cancellationToken);

        var result = await mediator.Send(
            new AnalyzeFoodPhotoCommand(ms.ToArray(), photo.ContentType),
            cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { result.Error.Code, result.Error.Description });

        return Ok(result.Value);
    }
}
