using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;

public class LeavingReasonsControllerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task GetRegions_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] LeavingReasonsController sut,
        GetLeavingReasonsQueryResult response,
        CancellationToken cancellationToken)
    {

        mediatorMock.Setup(m => m.Send(It.IsAny<GetLeavingReasonsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.GetLeavingReasons(cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();

        result.As<OkObjectResult>()?.Value.Should().BeEquivalentTo(response.ProcessedResult);
    }
}