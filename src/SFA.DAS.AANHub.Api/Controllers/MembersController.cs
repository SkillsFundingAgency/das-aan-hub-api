using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.SwaggerExamples;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Application.Members.Queries.GetMemberByEmail;
using SFA.DAS.AANHub.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class MembersController : ActionResponseControllerBase
{
    private readonly IMediator _mediator;

    private readonly ILogger<MembersController> _logger;

    public override string ControllerName => "Members";

    public MembersController(IMediator mediator, ILogger<MembersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
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

    [HttpGet]
    [Route("{email}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(GetMemberByEmailResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(string email)
    {
        _logger.LogInformation("AAN Hub API: Received command to get member by email: {email}", email);

        var response = await _mediator.Send(new GetMemberByEmailQuery(email));

        return GetResponse(response);
    }
}
