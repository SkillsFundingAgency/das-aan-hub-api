using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationLocations;

namespace SFA.DAS.AANHub.Api.Controllers;

[ApiController]
[Route("MemberNotificationLocations")]
public class MemberNotificationLocationsController : ActionResponseControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MemberNotificationLocationsController> _logger;

    public override string ControllerName => "MemberNotificationLocations";

    public MemberNotificationLocationsController(ILogger<MemberNotificationLocationsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{memberId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMemberNotificationLocations(
            [FromRoute] Guid memberId,
            CancellationToken cancellationToken)
    {
        _logger.LogInformation("AAN Hub API: Received command to get members notification locations by MemberId: {memberId}", memberId);

        var response = await _mediator.Send(new GetMemberNotificationLocationsQuery() { MemberId = memberId }, cancellationToken);

        return GetResponse(response);
    }
}