using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("Members")]
[ApiController]
public class MemberProfilesController : ActionResponseControllerBase
{
    private readonly IMediator _mediator;

    public override string ControllerName => "MemberProfiles";

    public MemberProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{memberId}/profile")]
    [ProducesResponseType(typeof(SuccessCommandResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutMemberProfile(
            [FromRoute] Guid memberId,
            [FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
            [FromBody] UpdateMemberProfileModel model)
    {
        UpdateMemberProfilesCommand command = new(memberId, requestedByMemberId, model.Profiles, model.Preferences);
        var response = await _mediator.Send(command);

        return GetPutResponse(response);
    }
}
