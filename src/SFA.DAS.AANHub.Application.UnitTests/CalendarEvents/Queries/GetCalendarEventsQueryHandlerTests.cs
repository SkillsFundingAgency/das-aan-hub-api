using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries;
using SFA.DAS.AANHub.Domain.Common;
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
         List<EventFormat> eventFormats,
         List<int> calendarId,
         CancellationToken cancellationToken
     )
    {
        var fromDate = DateTime.Today;
        var toDate = DateTime.Today.AddDays(7);

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(member.Id, fromDate, toDate, eventFormats, calendarId, cancellationToken))
        .ReturnsAsync(() => new List<CalendarEventSummary>());

        var result = await sut.Handle(new GetCalendarEventsQuery(member.Id, fromDate, toDate, eventFormats, calendarId, 1), new CancellationToken());

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
        List<EventFormat> eventFormats,
        List<int> calendarId,
        CancellationToken cancellationToken
    )
    {
        var fromDate = DateTime.Today;
        var toDate = DateTime.Today.AddDays(7);

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(member.Id, fromDate, toDate, eventFormats, calendarId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        var result = await sut.Handle(new GetCalendarEventsQuery(member.Id, fromDate, toDate, eventFormats, calendarId, 1), cancellationToken);

        result.Result.CalendarEvents.Count.Should().Be(1);
        result.Result.TotalCount.Should().Be(1);

        var calendarEvents = result.Result.CalendarEvents;

        calendarEvents.First().Should().BeEquivalentTo(calendarEvent, options => options.Excluding(c => c.CalendarId));
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_FromDateAfterToDate_ReturnsNoCalendarEvents_DoesNotCallRepository(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        GetCalendarEventsQueryHandler sut,
        CalendarEventSummary calendarEvent,
        Member member,
        List<EventFormat> eventFormats,
        List<int> calendarId,
        CancellationToken cancellationToken
    )
    {
        var fromDate = DateTime.Today.AddDays(7);
        var toDate = DateTime.Today;

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(member.Id, fromDate, toDate, eventFormats, calendarId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        var result = await sut.Handle(new GetCalendarEventsQuery(member.Id, fromDate, toDate, eventFormats, calendarId, 1), cancellationToken);

        result.Result.CalendarEvents.Count.Should().Be(0);
        result.Result.TotalCount.Should().Be(0);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<List<EventFormat>>(), It.IsAny<List<int>>(), cancellationToken), Times.Never);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_FromDateNull_FromDateIsUpdatedToToday(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        GetCalendarEventsQueryHandler sut,
        CalendarEventSummary calendarEvent,
        Member member,
        List<EventFormat> eventFormats,
        List<int> calendarId,
        CancellationToken cancellationToken
    )
    {
        DateTime? fromDate = null;
        var toDate = DateTime.Today;

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(member.Id, DateTime.Today, toDate, eventFormats, calendarId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        await sut.Handle(new GetCalendarEventsQuery(member.Id, fromDate, toDate, eventFormats, calendarId, 1), cancellationToken);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(It.IsAny<Guid>(), DateTime.Today, toDate, It.IsAny<List<EventFormat>>(), calendarId, cancellationToken), Times.Once);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_FromDateBeforeToday_FromDateIsUpdatedToToday(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        GetCalendarEventsQueryHandler sut,
        CalendarEventSummary calendarEvent,
        Member member,
        CancellationToken cancellationToken
    )
    {
        var eventFormats = new List<EventFormat>();
        var calendarId = new List<int>();
        var fromDate = DateTime.Today.AddDays(-7);
        var toDate = DateTime.Today;

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(member.Id, DateTime.Today, toDate, eventFormats, calendarId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        await sut.Handle(new GetCalendarEventsQuery(member.Id, fromDate, toDate, eventFormats, calendarId, 1), cancellationToken);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(It.IsAny<Guid>(), DateTime.Today, toDate, It.IsAny<List<EventFormat>>(), calendarId, cancellationToken), Times.Once);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_ToDateNull_ToDateIsUpdatedToTodayPlusOneYear(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        GetCalendarEventsQueryHandler sut,
        CalendarEventSummary calendarEvent,
        Member member,
        CancellationToken cancellationToken
    )
    {
        var eventFormats = new List<EventFormat>();
        var calendarId = new List<int>();
        var fromDate = DateTime.Today;
        DateTime? toDate = null;
        var todayPlusOneYear = DateTime.Today.AddYears(1);

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(member.Id, DateTime.Today, todayPlusOneYear, eventFormats, calendarId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        await sut.Handle(new GetCalendarEventsQuery(member.Id, fromDate, toDate, eventFormats, calendarId, 1), cancellationToken);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(It.IsAny<Guid>(), DateTime.Today, todayPlusOneYear, new List<EventFormat>(), calendarId, cancellationToken), Times.Once);
    }
}