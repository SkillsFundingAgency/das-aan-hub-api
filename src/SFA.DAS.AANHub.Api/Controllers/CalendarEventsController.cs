using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarEventsController : ActionResponseControllerBase
{
    private readonly ILogger<CalendarEventsController> _logger;
    private readonly IMediator _mediator;

    public override string ControllerName => "CalendarEvents";

    public CalendarEventsController(ILogger<CalendarEventsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{calendareventid}")]
    [ProducesResponseType(typeof(GetMemberResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid calendarEventId, [FromHeader(Name = "X-RequestedByUser")] Guid requestedByUserId)
    {
        _logger.LogInformation("AAN Hub API: Received command from User ID {requestedByUserId} to get calendar event by event ID {calendarEventId}", requestedByUserId, calendarEventId);

        var response = await _mediator.Send(new GetCalendarEventByIdQuery(calendarEventId, requestedByUserId));

        if (!response.IsValidResponse)
        {
            foreach (var errorMessage in response.Errors.Select(e => e.ErrorMessage))
            {
                _logger.LogError("AAN Hub API: {errorMessage}. Request came from User ID {requestedByuserId}", errorMessage, requestedByUserId);
            }
        }

        return GetResponse(response);
    }
}
