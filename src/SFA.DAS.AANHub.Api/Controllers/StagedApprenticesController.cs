using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Application.StagedApprentices.Queries;

namespace SFA.DAS.AANHub.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class StagedApprenticesController : ActionResponseControllerBase
{
    private readonly IMediator _mediator;

    public override string ControllerName => "StagedApprentices";

    public StagedApprenticesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(GetStagedApprenticeQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStagedApprentice(GetStagedApprenticeQuery query, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return GetResponse(response);
    }
}
