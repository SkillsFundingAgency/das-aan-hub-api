using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.MemberNotificationEventFormats.Queries.GetMemberNotificationEventFormats;

namespace SFA.DAS.AANHub.Api.Controllers;

[ApiController]
[Route("MemberNotificationEventFormats")]
public class MemberNotificationEventFormatsController : ActionResponseControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MemberNotificationEventFormatsController> _logger;

    public override string ControllerName => "MemberNotificationEventFormats";

    public MemberNotificationEventFormatsController(ILogger<MemberNotificationEventFormatsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{memberId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMemberNotificationEventFormats(
            [FromRoute] Guid memberId,
            CancellationToken cancellationToken)
    {
        _logger.LogInformation("AAN Hub API: Received command to get members notification event formats by MemberId: {memberId}", memberId);

        var response = await _mediator.Send(new GetMemberNotificationEventFormatsQuery() { MemberId = memberId }, cancellationToken);

        return GetResponse(response);
    }
}