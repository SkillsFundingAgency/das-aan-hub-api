using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Calendars.Queries.GetCalendars;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;

public class CalendarsControllerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task GetCalendars_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarsController sut,
        GetCalendarsQueryResult calendars,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<GetCalendarsQueryResult>(calendars);

        mediatorMock.Setup(m => m.Send(It.IsAny<GetCalendarsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.GetCalendars(cancellationToken);

        sut.ControllerName.Should().Be("Calendars");
        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>()?.Value.Should().Be(calendars);
    }

}