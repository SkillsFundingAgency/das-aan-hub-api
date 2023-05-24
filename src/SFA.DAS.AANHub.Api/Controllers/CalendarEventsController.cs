using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AANHub.Application.Common;

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

    [HttpGet]
    [ProducesResponseType(typeof(GetCalendarEventsQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCalendarEvents([FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("AAN Hub API: Received command from User ID {requestedByMemberId} to get calendar events", requestedByMemberId);
        var page = 1;
        var response = await _mediator.Send(new GetCalendarEventsQuery(requestedByMemberId, page), cancellationToken);

        return GetResponse(response);
    }

    [HttpPut("{calendarEventId}/attendance")]
    [ProducesResponseType(typeof(SuccessCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutAttendance(
            Guid calendarEventId,
            [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
            [FromBody] PutAttendanceModel model)
    {
        _logger.LogInformation("AAN Hub API: Received command from Member Id {requestedByMemberId} to PUT Attendance with Active = {IsAttending} on Calendar Event ID {calendarEventId}",
            requestedByMemberId,
            model.IsAttending,
            calendarEventId);

        var command = new PutAttendanceCommand(calendarEventId, requestedByMemberId, model.IsAttending);
        var response = await _mediator.Send(command);

        return GetPutResponse(response);
    }
}

public record struct PutAttendanceModel(bool IsAttending);
