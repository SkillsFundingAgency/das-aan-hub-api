﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;
using SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("Members")]
[ApiController]
public class MemberProfilesController : ActionResponseControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MemberProfilesController> _logger;

    public override string ControllerName => "MemberProfiles";

    public MemberProfilesController(ILogger<MemberProfilesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{memberId}/profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMemberProfileWithPreferences(
            [FromRoute] Guid memberId,
            [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
            CancellationToken cancellationToken,
            bool @public = true)
    {
        _logger.LogInformation("AAN Hub API: Received command to get members profile and preferences by MemberId: {memberId}", memberId);

        var response = await _mediator.Send(new GetMemberProfilesWithPreferencesQuery() { RequestedByMemberId = requestedByMemberId, MemberId = memberId, IsPublicView = @public }, cancellationToken);

        return GetResponse(response);
    }

    [HttpPut("{memberId}/profile")]
    [ProducesResponseType(typeof(SuccessCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutMemberProfile(
        [FromRoute] Guid memberId,
        [FromBody] UpdateMemberProfileModel model,
        CancellationToken cancellationToken)
    {
        UpdateMemberProfilesCommand command = new(memberId, model.MemberProfiles, model.MemberPreferences);
        var response = await _mediator.Send(command, cancellationToken);

        return GetPutResponse(response);
    }
}
