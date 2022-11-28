
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AAN.Application.Commands.CreateCalendarEvent;
using SFA.DAS.AAN.Application.Commands.DeleteCalendarEvent;
using SFA.DAS.AAN.Application.Commands.PatchCalendarEvent;
using SFA.DAS.AAN.Application.Queries.GetCalendars;
using SFA.DAS.AAN.Application.Queries.GetCalendarsForUser;
using SFA.DAS.AAN.Application.Queries.GetCalendarEvents;


namespace SFA.DAS.AAN.Hub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class CalendarController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(IMediator mediator, ILogger<CalendarController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCalendar(Guid? memberId)
        {
            try
            {
                object result;
                if (!memberId.HasValue)
                    result = await _mediator.Send(new GetCalendarsQuery()) as IEnumerable<GetCalendarsResultItem>;
                else
                    result = await _mediator.Send(new GetCalendarsForUserQuery() { MemberId = memberId.Value }) as GetCalendarsForUserResult;
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Calendars {(memberId.HasValue ? "for member " + memberId.ToString() : "")}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("{calendarid}/calendarevent")]
        public async Task<IActionResult> CreateCalendarEvent([FromRoute] long calendarid, [FromBody] CreateCalendarEventCommand command)
        {
            try
            {
                command.calendarid = calendarid;
                CreateCalendarEventResponse result = await _mediator.Send(command) as CreateCalendarEventResponse;
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to create event for Calendar {calendarid}");
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("{calendarid}/calendarevent/{calendareventid}")]
        public async Task<IActionResult> PatchCalendarEvent([FromRoute] long calendarid, [FromRoute] Guid calendareventid, [FromBody] PatchCalendarEventCommand command)
        {
            try
            {
                command.calendarid = calendarid;
                command.calendareventid = calendareventid;
                PatchCalendarEventResponse result = await _mediator.Send(command) as PatchCalendarEventResponse;

                if (result?.warnings != null && result.warnings.Any())
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to patch event {calendareventid} for Calendar {calendarid}");
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{calendarid}/calendarevent/{calendareventid}")]
        public async Task<IActionResult> DeleteCalendarEvent([FromRoute] long calendarid, [FromRoute] Guid calendareventid, [FromBody] DeleteCalendarEventCommand command)
        {
            try
            {
                command.calendareventid = calendareventid;
                DeleteCalendarEventResponse result = await _mediator.Send(command) as DeleteCalendarEventResponse;

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to delete event {calendareventid} for Calendar {calendarid}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{calendarid}/calendarevents")]
        public async Task<IActionResult> GetCalendarEvents(
            [FromRoute] long calendarid,
            [FromQuery] Guid memberid,
            [FromQuery] string? calendareventid,
            [FromQuery] string? region,
            [FromQuery] DateTime fromdate,
            [FromQuery] DateTime todate)
        {
            try
            {
                List<string> warnings = new List<string>();
                if (memberid == null || memberid == Guid.Empty)
                    warnings.Add("memberid missing");
                if (fromdate == DateTime.MinValue)
                    warnings.Add("fromdate missing");
                if (todate == DateTime.MinValue)
                    warnings.Add("todate missing");

                if (warnings.Any())
                    return BadRequest(new { errors = warnings });

                IEnumerable<GetCalendarEventsResultItem> result =
                    await _mediator.Send(new GetCalendarEventsQuery()
                    {
                        memberid = memberid,
                        calendarid = calendarid,
                        calendareventid = calendareventid,
                        region = region,
                        fromdate = fromdate,
                        todate = todate
                    }) as IEnumerable<GetCalendarEventsResultItem>;
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get Calendar events for member {memberid.ToString()}");
                return BadRequest();
            }
        }

    }
}
