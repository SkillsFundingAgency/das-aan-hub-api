﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Common;
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

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(GetMemberResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] Guid id, [FromHeader] Guid requestedByMemberId)
    {
        _logger.LogInformation("AAN Hub API: Received command to get notification by Id: {notificationId}", id);

        var response = await _mediator.Send(new GetNotificationQuery(id, requestedByMemberId));

        return GetResponse(response);
    }
}
