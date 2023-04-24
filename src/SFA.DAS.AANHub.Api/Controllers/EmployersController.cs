using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;
using SFA.DAS.AANHub.Application.Employers.Queries;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class EmployersController : ActionResponseControllerBase
{
    private readonly ILogger<EmployersController> _logger;
    private readonly IMediator _mediator;

    public override string ControllerName => "Employers";

    public EmployersController(ILogger<EmployersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEmployer(CreateEmployerMemberCommand command)
    {
        _logger.LogInformation("AAN Hub API: Received command to add employer by accountId: {accountId} and UserRef: {UserRef}.", command.AccountId, command.UserRef);

        var response = await _mediator.Send(command);

        return GetPostResponse(response, new { userRef = command.UserRef });
    }

    [HttpGet]
    [Route("{userRef}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(GetMemberResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid userRef)
    {
        _logger.LogInformation("AAN Hub API: Received command to get employer by UserRef: {userRef}", userRef);

        var response = await _mediator.Send(new GetEmployerMemberQuery(userRef));

        return GetResponse(response);
    }
}
