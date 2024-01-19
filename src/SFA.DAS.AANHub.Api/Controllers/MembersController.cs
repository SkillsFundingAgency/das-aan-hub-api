using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Api.SwaggerExamples;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberLeaving;
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberReinstate;
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberRemove;
using SFA.DAS.AANHub.Application.Members.Queries.GetMember;
using SFA.DAS.AANHub.Application.Members.Queries.GetMemberByEmail;
using SFA.DAS.AANHub.Application.Members.Queries.GetMembers;
using SFA.DAS.AANHub.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;
using Constants = SFA.DAS.AANHub.Api.Common.Constants;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class MembersController : ActionResponseControllerBase
{
    private readonly ILogger<MembersController> _logger;
    private readonly IMediator _mediator;

    public override string ControllerName => "Members";

    public MembersController(ILogger<MembersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetMembersQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMembers([FromQuery] GetMembersModel model, CancellationToken cancellationToken)
    {
        if (model.Page <= 0)
        {
            model.Page = 1;
        }

        if (model.PageSize <= 0)
        {
            model.PageSize = Domain.Common.Constants.Members.PageSize;
        }

        var response = await _mediator.Send((GetMembersQuery)model, cancellationToken);

        return new OkObjectResult(response);
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
    [ProducesResponseType(typeof(GetMemberResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(string email)
    {
        _logger.LogInformation("AAN Hub API: Received command to get member by email: {email}", email);

        var response = await _mediator.Send(new GetMemberByEmailQuery(email));

        return GetResponse(response);
    }

    [HttpGet]
    [Route("{memberId:guid}")]
    [ProducesResponseType(typeof(GetMemberResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMember(Guid memberId)
    {
        _logger.LogInformation("AAN Hub API: Received command to get member by memberId: {memberId}", memberId);

        var response = await _mediator.Send(new GetMemberQuery(memberId));

        return GetResponse(response);
    }

    [HttpPost]
    [Route("{memberId}/remove")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostMemberStatus([FromHeader(Name = Constants.RequestHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, [FromRoute] Guid memberId, [FromBody] PostMemberStatusModel request, CancellationToken cancellationToken)
    {
        PostMemberRemoveCommand command = new()
        {
            MemberId = memberId,
            AdminMemberId = requestedByMemberId,
            Status = request.Status
        };

        var response = await _mediator.Send(command, cancellationToken);

        if (response.Result is { IsSuccess: false }) return NotFound();
        if (response.IsValidResponse) return NoContent();

        return new BadRequestObjectResult(FormatErrors(response.Errors));
    }

    [HttpPost]
    [Route("{memberId}/leaving")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostMemberLeaving([FromRoute] Guid memberId, [FromBody] PostMemberLeavingModel request, CancellationToken cancellationToken)
    {
        PostMemberLeavingCommand command = new()
        {
            MemberId = memberId,
            LeavingReasons = request.LeavingReasons
        };

        var response = await _mediator.Send(command, cancellationToken);

        if (response.Result is { IsSuccess: false }) return NotFound();
        if (response.IsValidResponse) return NoContent();

        return new BadRequestObjectResult(FormatErrors(response.Errors));
    }


    [HttpPost]
    [Route("{memberId}/reinstate")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostMemberReinstate([FromRoute] Guid memberId, CancellationToken cancellationToken)
    {
        PostMemberReinstateCommand command = new()
        {
            MemberId = memberId
        };

        var response = await _mediator.Send(command, cancellationToken);

        if (response.Result is { IsSuccess: false }) return NotFound();
        if (response.IsValidResponse) return NoContent();

        return new BadRequestObjectResult(FormatErrors(response.Errors));
    }
}
