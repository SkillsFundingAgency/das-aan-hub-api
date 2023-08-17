using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Notifications.Queries;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class NotificationsController : ActionResponseControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<NotificationsController> _logger;

    public override string ControllerName => "Notifications";

    public NotificationsController(IMediator mediator, ILogger<NotificationsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(GetNotificationQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] Guid id, [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("AAN Hub API: Received command to get notification by Id: {notificationId}", id);

        var response = await _mediator.Send(new GetNotificationQuery(id, requestedByMemberId), cancellationToken);

        return GetResponse(response);
    }
}
