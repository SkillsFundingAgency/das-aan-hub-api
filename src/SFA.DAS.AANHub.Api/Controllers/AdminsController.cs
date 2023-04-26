using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;
using SFA.DAS.AANHub.Application.Admins.Queries;
using SFA.DAS.AANHub.Application.Common;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AdminsController : ActionResponseControllerBase
{
    private readonly ILogger<AdminsController> _logger;
    private readonly IMediator _mediator;

    public override string ControllerName => "Admins";

    public AdminsController(ILogger<AdminsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAdmin(CreateAdminMemberCommand command)
    {
        var response = await _mediator.Send(command);

        return GetPostResponse(response, new { userName = command.UserName });
    }

    [HttpGet]
    [Route("{userName}")]
    [ProducesResponseType(typeof(GetMemberResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string userName)
    {
        _logger.LogInformation("AAN Hub API: Received command to get Admin by Username: {userName}", userName);

        var response = await _mediator.Send(new GetAdminMemberQuery(userName));

        return GetResponse(response);
    }
}
