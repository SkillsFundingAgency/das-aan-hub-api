using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;

namespace SFA.DAS.AANHub.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LeavingReasonsController
{
    private readonly ILogger<LeavingReasonsController> _logger;
    private readonly IMediator _mediator;


    public LeavingReasonsController(ILogger<LeavingReasonsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<LeavingCategory>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLeavingReasons(CancellationToken cancellationToken)
    {
        _logger.LogTrace("Requesting list of leaving reasons");

        var result = await _mediator.Send(new GetLeavingReasonsQuery(), cancellationToken);

        return new OkObjectResult(result);
    }
}