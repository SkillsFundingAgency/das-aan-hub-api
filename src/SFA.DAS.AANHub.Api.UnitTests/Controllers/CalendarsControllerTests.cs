using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Calendars.Queries.GetCalendars;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;

public class CalendarsControllerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task GetCalendars_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarsController sut,
        GetCalendarsQueryResult result,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetCalendarsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var response = await sut.GetCalendars(cancellationToken);

        response.Should().NotBeNull();
        response.Count().Should().Be(result.Calendars.Count());
        response.Should().BeEquivalentTo(result.Calendars);
    }

}