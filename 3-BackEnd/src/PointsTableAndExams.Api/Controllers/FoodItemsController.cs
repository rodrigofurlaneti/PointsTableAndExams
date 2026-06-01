using MediatR;
using Microsoft.AspNetCore.Mvc;
using PointsTableAndExams.Application.FoodItems.Queries.SearchFoodItems;

namespace PointsTableAndExams.Api.Controllers;

[Route("api/food-items")]
public sealed class FoodItemsController(IMediator mediator) : BaseApiController(mediator)
{
    /// <summary>List food items. Optional ?search= filter by name.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? search, CancellationToken ct)
    {
        var result = await Mediator.Send(new SearchFoodItemsQuery(search), ct);
        return FromResult(result);
    }
}
