using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.HealthCheck;
using SFA.DAS.AANHub.Application.RegionsCount;

namespace SFA.DAS.AANHub.Api.UnitTests.Services.HealthChecks;

[TestFixture]
public class RegionsHealthCheckTests
{
    private Mock<IMediator> _mediatr = null!;
    private RegionsHealthCheck _regionsHealthCheck = null!;

    [SetUp]
    public void Before_each_test()
    {
        _mediatr = new Mock<IMediator>();
        _regionsHealthCheck = new RegionsHealthCheck(_mediatr.Object);
    }

    [Test]
    public async Task RegionsHealthCheck_RegionsPresent_ReturnsHealthy()
    {
        var regionsCount = 1;
        _mediatr.Setup(x => x.Send(It.IsAny<RegionsCountRequest>(), new CancellationToken())).ReturnsAsync(regionsCount);
        var result = await _regionsHealthCheck.CheckHealthAsync(new HealthCheckContext());
        Assert.AreEqual(HealthStatus.Healthy, result.Status);
        _mediatr.Verify(x => x.Send(It.IsAny<RegionsCountRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task RegionsHealthCheck_RegionsAbsent_ReturnsUnhealthy()
    {
        var regionsCount = 0;
        _mediatr.Setup(x => x.Send(It.IsAny<RegionsCountRequest>(), new CancellationToken())).ReturnsAsync(regionsCount);
        var result = await _regionsHealthCheck.CheckHealthAsync(new HealthCheckContext());
        Assert.AreEqual(HealthStatus.Unhealthy, result.Status);
        _mediatr.Verify(x => x.Send(It.IsAny<RegionsCountRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
