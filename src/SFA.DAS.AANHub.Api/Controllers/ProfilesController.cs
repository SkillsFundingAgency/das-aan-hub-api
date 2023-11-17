using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProfilesController : ActionResponseControllerBase
{
    private readonly ILogger<ProfilesController> _logger;
    private readonly IMediator _mediator;

    public override string ControllerName => "Profiles";

    public ProfilesController(ILogger<ProfilesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Get list of profiles by user type
    /// </summary>
    /// <param name="userType"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{userType}")]
    [ProducesResponseType(typeof(GetProfilesByUserTypeQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfilesByUserType([FromRoute] UserType userType)
    {
        _logger.LogInformation("AAN Hub API: Received command to get profiles");

        var result = await _mediator.Send(new GetProfilesByUserTypeQuery(userType));

        return Ok(result);
    }

}