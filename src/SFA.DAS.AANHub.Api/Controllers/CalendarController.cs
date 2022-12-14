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
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetCalendarsResultItem), 200)]
        [ProducesResponseType(typeof(GetCalendarsForUserResultItem), 200)]
        public async Task<IActionResult> GetCalendar(Guid? memberId)
        {
            object result;
            if (!memberId.HasValue)
            {
                result = await _mediator.Send(new GetCalendarsQuery());
                _logger.LogInformation("All calendars returned");
            }

            else
            {
                result = await _mediator.Send(new GetCalendarsForUserQuery() { MemberId = memberId.Value });
                _logger.LogInformation("All calendars returned for user {memberId}", memberId);
            }

            return Ok(result);

        }
    }
}
