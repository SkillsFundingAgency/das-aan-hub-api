using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.AANHub.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RegionsController : ActionResponseControllerBase
{
    private readonly ILogger<RegionsController> _logger;
    private readonly IMediator _mediator;

    public override string ControllerName => "Regions";

    public RegionsController(IMediator mediator, ILogger<RegionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(GetRegionsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRegions()
    {
        _logger.LogTrace("Requesting list of regions");

        var result = await _mediator.Send(new GetRegionsQuery());

        return GetResponse(result);
    }
}
