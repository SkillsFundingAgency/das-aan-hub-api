using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Calendars.Queries.GetCalendars;
using SFA.DAS.AANHub.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.AANHub.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarsController : ActionResponseControllerBase
{
    private readonly ILogger<CalendarsController> _logger;
    private readonly IMediator _mediator;

    public override string ControllerName => "Calendars";

    public CalendarsController(ILogger<CalendarsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(GetRegionsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCalendars(CancellationToken cancellationToken)
    {
        _logger.LogTrace("Requesting list of calendars");

        var result = await _mediator.Send(new GetCalendarsQuery(), cancellationToken);

        return GetResponse(result);
    }
}
