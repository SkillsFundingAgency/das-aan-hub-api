﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.StagedApprentices.Queries;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class StagedApprenticesController : ActionResponseControllerBase
{
    private readonly ILogger<StagedApprenticesController> _logger;
    private readonly IMediator _mediator;

    public override string ControllerName => "StagedApprentices";

    public StagedApprenticesController(ILogger<StagedApprenticesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(GetStagedApprenticeQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStagedApprentice([FromQuery] string lastName, [FromQuery] DateTime dateOfBirth, [FromQuery] string email)
    {
        _logger.LogInformation("AAN Hub API: Received command to get StagedApprentice for Email: {email}", email);

        var response = await _mediator.Send(new GetStagedApprenticeQuery(lastName, dateOfBirth, email));
        return GetResponse(response);
    }
}
