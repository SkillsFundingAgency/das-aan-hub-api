using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.MemberNotificationLocations.Commands.UpdateMemberNotificationLocations;
using SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationLocations;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
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

        return new OkObjectResult(response);
    }

    [HttpPost("{memberId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PostMemberNotificationLocations(
        [FromRoute] Guid memberId,
        [FromBody] UpdateMemberNotificationLocationsApiRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("AAN Hub API: Received command to update notification locations by MemberId: {memberId}", memberId);

        var command = new UpdateMemberNotificationLocationsCommand
        {
            MemberId = memberId,
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