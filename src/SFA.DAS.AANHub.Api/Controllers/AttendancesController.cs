using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Attendances.Queries.GetMemberAttendances;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AttendancesController : ActionResponseControllerBase
{
    private readonly IMediator _mediator;

    public AttendancesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override string ControllerName => "Attendances";

    [HttpGet]
    [ProducesResponseType(typeof(GetMemberAttendancesQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(
        [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        CancellationToken cancellationToken)
    {
        GetMemberAttendancesQuery query = new(requestedByMemberId, fromDate, toDate);
        var response = await _mediator.Send(query, cancellationToken);
        return GetResponse(response);
    }
}
