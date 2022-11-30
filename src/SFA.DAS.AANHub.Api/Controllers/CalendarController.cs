using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Queries.GetCalendars;
using SFA.DAS.AANHub.Application.Queries.GetCalendarsForUser;

namespace SFA.DAS.AANHub.Api.Controllers
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
                    result = await _mediator.Send(new GetCalendarsQuery());
                else
                    result = await _mediator.Send(new GetCalendarsForUserQuery() { MemberId = memberId.Value });
                return Ok(result);
            }
            catch (Exception e)
            {
                var error =
                    $"Error attempting to get Calendars {(memberId.HasValue ? "for member " + memberId.ToString() : "")}";
                _logger.LogError(e, "{error}", error);
                return BadRequest();
            }
        }
    }
}
