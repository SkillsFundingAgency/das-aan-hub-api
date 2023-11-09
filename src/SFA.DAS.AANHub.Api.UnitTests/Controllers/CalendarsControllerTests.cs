using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AANHub.Application.Calendars.Queries.GetCalendars;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;

public class CalendarsControllerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task GetCalendars_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarsController sut,
        GetCalendarsQueryResult result,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetCalendarsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(result);

        var getResult = await sut.GetCalendars(cancellationToken);

        var response = getResult as OkObjectResult;
        var responseCalendars = (IEnumerable<CalendarModel>)response!.Value!;

        var calendarModels = responseCalendars.ToList();
        calendarModels.Should().NotBeNull();
        calendarModels.Count.Should().Be(result.Calendars.Count());
        calendarModels.Should().BeEquivalentTo(result.Calendars);
    }

}