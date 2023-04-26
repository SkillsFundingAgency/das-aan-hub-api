using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;
using SFA.DAS.AANHub.Application.Partners.Queries;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PartnersController : ActionResponseControllerBase
{
    private readonly ILogger<PartnersController> _logger;
    private readonly IMediator _mediator;

    public override string ControllerName => "Partners";

    public PartnersController(ILogger<PartnersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePartner(CreatePartnerMemberCommand command)
    {
        var response = await _mediator.Send(command);

        return GetPostResponse(response, new { userName = command.UserName });
    }

    [HttpGet]
    [Route("{userName}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(GetMemberResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(string userName)
    {
        _logger.LogInformation("AAN Hub API: Received command to get partner by userName: {userName}", userName);

        var response = await _mediator.Send(new GetPartnerMemberQuery(userName));

        return GetResponse(response);
    }
}
