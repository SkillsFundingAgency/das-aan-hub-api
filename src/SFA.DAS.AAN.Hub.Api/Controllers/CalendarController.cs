﻿
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AAN.Application.Commands.CreateCalendarEvent;
using SFA.DAS.AAN.Application.Queries.GetCalendars;
using SFA.DAS.AAN.Application.Queries.GetCalendarsForUser;


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

    }
}
