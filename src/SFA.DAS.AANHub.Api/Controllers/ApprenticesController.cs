using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Application.Apprentices.Queries;
using SFA.DAS.AANHub.Application.Common;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ApprenticesController : ActionResponseControllerBase
{
    private readonly ILogger<ApprenticesController> _logger;
    private readonly IMediator _mediator;

    public override string ControllerName => "Apprentices";

    public ApprenticesController(ILogger<ApprenticesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CreateMemberCommandResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateApprentice(CreateApprenticeMemberCommand command)
    {
        var response = await _mediator.Send(command);

        return GetPostResponse(response, new { apprenticeId = command.ApprenticeId });
    }

    [HttpGet]
    [Route("{apprenticeId}")]
    [ProducesResponseType(typeof(GetMemberResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid apprenticeId)
    {
        _logger.LogInformation("AAN Hub API: Received command to get apprentice by ApprenticeId: {apprenticeId}", apprenticeId);

        var response = await _mediator.Send(new GetApprenticeMemberQuery(apprenticeId));

        return GetResponse(response);
    }
}