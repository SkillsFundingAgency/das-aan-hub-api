using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.AANHub.Application.RegionsCount;

namespace SFA.DAS.AANHub.Api.HealthCheck;

public class RegionsHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Regions Health Check";
    private readonly IMediator _mediator;
    public RegionsHealthCheck(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var regionsCount = await _mediator.Send(new RegionsCountRequest(), cancellationToken);
        return regionsCount == 0
            ? HealthCheckResult.Unhealthy(HealthCheckResultDescription)
            : HealthCheckResult.Healthy(HealthCheckResultDescription);
    }
}
