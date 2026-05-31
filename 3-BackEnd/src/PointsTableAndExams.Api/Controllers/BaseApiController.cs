using MediatR;
using Microsoft.AspNetCore.Mvc;
using PointsTableAndExams.Domain.Common;

namespace PointsTableAndExams.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator Mediator = mediator;

    protected IActionResult FromResult<T>(Result<T> result) =>
        result.IsSuccess ? Ok(result.Value) : Problem(result.Error);

    protected IActionResult FromResult(Result result) =>
        result.IsSuccess ? NoContent() : Problem(result.Error);

    private IActionResult Problem(Error error) => error.Code switch
    {
        "General.NotFound" => NotFound(new { error.Code, error.Description }),
        _ => BadRequest(new { error.Code, error.Description })
    };
}
