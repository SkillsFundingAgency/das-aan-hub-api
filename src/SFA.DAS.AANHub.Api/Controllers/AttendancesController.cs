using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Attendances.Commands;
using SFA.DAS.AANHub.Application.Attendances.Commands.CreateAttendance;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Employers.Queries;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AttendancesController : ActionResponseControllerBase
{
    private readonly ILogger<AttendancesController> _logger;
    private readonly IMediator _mediator;

    public override string ControllerName => "Attendances";

    public AttendancesController(ILogger<AttendancesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CreateAttendanceCommandResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAttendance(Guid calendarEventId, [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId)
    {
        _logger.LogInformation("AAN Hub API: Received command from Member Id {requestedByMemberId} to create Attendance for Calendar Event ID {calendarEventId}", requestedByMemberId, calendarEventId);

        var command = new CreateAttendanceCommand(calendarEventId, requestedByMemberId);
        var response = await _mediator.Send(command);

        var postResposne = GetPostResponse(response, new { attendanceId = command.Id });
        return postResposne;
    }
}
