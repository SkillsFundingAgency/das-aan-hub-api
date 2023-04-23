using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Regions.Queries.GetRegions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;

public class RegionsControllerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task GetRegions_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RegionsController sut,
        GetRegionsQueryResult regions)
    {
        var response = new ValidatedResponse<GetRegionsQueryResult>(regions);

        mediatorMock.Setup(m => m.Send(It.IsAny<GetRegionsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.GetRegions();

        result.As<OkObjectResult>().Should().NotBeNull();

        result.As<OkObjectResult>()?.Value.Should().Be(regions);
    }
}