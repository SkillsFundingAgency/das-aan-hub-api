using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Queries;
public class GetCalendarEventsQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_CalendarEventsNotFound_ReturnsEmptyList(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        GetCalendarEventsQueryHandler sut,
        Member member,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken
        )
    {
        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(member.Id, startDate, endDate, cancellationToken))
            .ReturnsAsync(() => new List<CalendarEventSummary>());

        var result = await sut.Handle(new GetCalendarEventsQuery(member.Id, startDate, endDate, 1), new CancellationToken());

        result.Result.CalendarEvents.Count.Should().Be(0);
        result.Result.TotalCount.Should().Be(0);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_CalendarEventsFound_ReturnsCalendarEvents(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        GetCalendarEventsQueryHandler sut,
        CalendarEventSummary calendarEvent,
        Member member,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken
    )
    {
        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(member.Id, startDate, endDate, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        var result = await sut.Handle(new GetCalendarEventsQuery(member.Id, startDate, endDate, 1), new CancellationToken());

        result.Result.CalendarEvents.Count.Should().Be(1);
        result.Result.TotalCount.Should().Be(1);

        var calendarEvents = result.Result.CalendarEvents;

        calendarEvents.First().Should().BeEquivalentTo(calendarEvent);
    }
}
