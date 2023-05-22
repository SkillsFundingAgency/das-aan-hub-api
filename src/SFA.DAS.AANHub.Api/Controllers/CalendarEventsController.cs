using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using Swashbuckle.AspNetCore.Filters;

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
    [Route("{calendarEventId}")]
    [ProducesResponseType(typeof(GetMemberResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid calendarEventId, [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId)
    {
        _logger.LogInformation("AAN Hub API: Received command from User ID {requestedByMemberId} to get calendar event by event ID {calendarEventId}", requestedByMemberId, calendarEventId);

        var response = await _mediator.Send(new GetCalendarEventByIdQuery(calendarEventId, requestedByMemberId));

        return GetResponse(response);
    }

    [HttpPut("{calendarEventId}/attendance")]
    [ProducesResponseType(typeof(SuccessCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(bool), typeof(bool))]
    public async Task<IActionResult> PutAttendance(
        Guid calendarEventId, 
        [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, 
        [FromBody] bool requestedActiveStatus)
    {
        _logger.LogInformation("AAN Hub API: Received command from Member Id {requestedByMemberId} to PUT Attendance with Active = {requestedActiveStatus} on Calendar Event ID {calendarEventId}", 
            requestedByMemberId, 
            requestedActiveStatus,
            calendarEventId);

        var command = new PutAttendanceCommand(calendarEventId, requestedByMemberId, requestedActiveStatus);
        var response = await _mediator.Send(command);

        return GetPutResponse(response);
    }
}
