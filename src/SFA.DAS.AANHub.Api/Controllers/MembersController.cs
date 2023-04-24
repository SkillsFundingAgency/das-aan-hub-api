using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.SwaggerExamples;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class MembersController : ActionResponseControllerBase
{
    private readonly IMediator _mediator;

    public override string ControllerName => "Members";

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPatch]
    [Route("{memberId}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    [SwaggerRequestExample(typeof(JsonPatchDocument), typeof(PatchMemberExample))]
    public async Task<IActionResult> PatchMember([FromRoute] Guid memberId, [FromBody] JsonPatchDocument<Member> request, CancellationToken cancellationToken)
    {
        PatchMemberCommand command = new()
        {
            MemberId = memberId,
            PatchDoc = request
        };

        var response = await _mediator.Send(command, cancellationToken);

        return GetPatchResponse(response);
    }
}
