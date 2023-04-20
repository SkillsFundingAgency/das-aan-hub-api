using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Application.Apprentices.Queries;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApprenticesController : ActionResponseControllerBase
{
    private const string ControllerName = "Apprentices";
    private readonly ILogger<ApprenticesController> _logger;
    private readonly IMediator _mediator;

    public ApprenticesController(ILogger<ApprenticesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    ///     Creates an apprentice member
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateApprentice(CreateApprenticeModel request)
    {
        CreateApprenticeMemberCommand command = request;

        var response = await _mediator.Send(command);

        return GetPostResponse(response,
            new ReferrerRouteDetails(
                nameof(GetApprentice),
                ControllerName,
                new RouteValueDictionary
                {
                    {
                        "apprenticeId", request.ApprenticeId
                    }
                }));
    }

    /// <summary>
    ///     Gets an apprentice member
    /// </summary>
    /// <param name="apprenticeId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{apprenticeId}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GetApprenticeMemberResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetApprentice(Guid apprenticeId)
    {
        _logger.LogInformation("AAN Hub API: Received command to get apprentice by ApprenticeId: {apprenticeId}", apprenticeId);

        var response = await _mediator.Send(new GetApprenticeMemberQuery(apprenticeId));
        return GetResponse(response);
    }
}