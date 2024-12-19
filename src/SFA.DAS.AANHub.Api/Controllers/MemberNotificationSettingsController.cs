using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.MemberNotificationLocations.Commands.UpdateMemberNotificationLocations;
using SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationLocations;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class MemberNotificationSettingsController : ActionResponseControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MemberNotificationSettingsController> _logger;

    public override string ControllerName => "MemberNotificationLocations";

    public MemberNotificationSettingsController(ILogger<MemberNotificationSettingsController> logger, IMediator mediator)
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
        //todo: this GET should now be obsolete

        _logger.LogInformation("AAN Hub API: Received command to get members notification locations by MemberId: {memberId}", memberId);

        var response = await _mediator.Send(new GetMemberNotificationLocationsQuery() { MemberId = memberId }, cancellationToken);

        return new OkObjectResult(response);
    }

    [HttpPost("{memberId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PostMemberNotificationSettings(
        [FromRoute] Guid memberId,
        [FromBody] UpdateMemberNotificationSettingsApiRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("AAN Hub API: Received command to update notification locations by MemberId: {memberId}", memberId);

        var command = new UpdateMemberNotificationLocationsCommand
        {
            MemberId = memberId,
            ReceiveNotifications = request.ReceiveNotifications,
            EventTypes = request.EventTypes.Select(x => new UpdateMemberNotificationLocationsCommand.NotificationEventType
            {
                EventType = x.EventType,
                Ordering = x.Ordering,
                ReceiveNotifications = x.ReceiveNotifications
            }).ToList(),
            Locations = request.Locations.Select(x => new UpdateMemberNotificationLocationsCommand.Location
            {
                Name = x.Name,
                Radius = x.Radius,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToList()
        };

        await _mediator.Send(command, cancellationToken);

        return new OkResult();
    }
}