using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.EventGuests.PutEventGuests;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using Constants = SFA.DAS.AANHub.Api.Common.Constants;

namespace SFA.DAS.AANHub.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarEventsController : ActionResponseControllerBase
{
    private readonly ILogger<CalendarEventsController> _logger;
    private readonly IMediator _mediator;
    private readonly ICalendarEventsReadRepository _calendarEventsReadRepository;

    public override string ControllerName => "CalendarEvents";

    public CalendarEventsController(ILogger<CalendarEventsController> logger, IMediator mediator, ICalendarEventsReadRepository calendarEventsReadRepository)
    {
        _logger = logger;
        _mediator = mediator;
        _calendarEventsReadRepository = calendarEventsReadRepository;
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
    public async Task<IActionResult> GetCalendarEvents([FromQuery] GetCalendarEventsModel model, CancellationToken cancellationToken)
    {
        _logger.LogInformation("AAN Hub API: Received command from User ID {requestedByMemberId} to get calendar events", model.RequestedByMemberId);
        if (model.Page <= 0)
        {
            model.Page = 1;
        }

        if (model.PageSize <= 0)
        {
            model.PageSize = Domain.Common.Constants.CalendarEvents.PageSize;
        }

        var response = await _mediator.Send((GetCalendarEventsQuery)model, cancellationToken);

        return new OkObjectResult(response);
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CreateCalendarEventCommandResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCalendarEvent(
        [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [FromBody] CreateCalendarEventModel model,
        CancellationToken cancellationToken)
    {
        CreateCalendarEventCommand command = model;
        command.AdminMemberId = requestedByMemberId;

        var result = await _mediator.Send(command, cancellationToken);

        return GetPostResponse(result, new { result.Result?.CalendarEventId });
    }

    [HttpPut("{calendarEventId}/eventguests")]
    [ProducesResponseType(typeof(SuccessCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutEventGuests(
        [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [FromRoute] Guid calendarEventId,
        [FromBody] PutEventGuestsModel model,
        CancellationToken cancellationToken)
    {
        var calendarEvent = await _calendarEventsReadRepository.GetCalendarEvent(calendarEventId);
        PutEventGuestsCommand command = new()
        {
            AdminMemberId = requestedByMemberId,
            CalendarEventId = calendarEventId,
            CalendarEvent = calendarEvent,
            Guests = model.Guests
        };

        var result = await _mediator.Send(command, cancellationToken);

        return GetPutResponse(result);
    }
}
