using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;
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
        Member member,
        List<EventFormat> eventFormats,
        List<int> calendarIds,
        List<int> regionIds,
        string keyword,
        CancellationToken cancellationToken
     )
    {
        var fromDate = DateTime.Today;
        var toDate = DateTime.Today.AddDays(7);

        var calendarEventsReadRepositoryMock = new Mock<ICalendarEventsReadRepository>();
        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(It.IsAny<GetCalendarEventsOptions>(), cancellationToken))
        .ReturnsAsync(() => new List<CalendarEventSummary>());

        var sut = new GetCalendarEventsQueryHandler(calendarEventsReadRepositoryMock.Object);

        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = member.Id,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            Page = 1,
            PageSize = 5
        };

        var result = await sut.Handle(query, new CancellationToken());

        result.Result.CalendarEvents.Count.Should().Be(0);
        result.Result.TotalCount.Should().Be(0);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_CalendarEventsFound_ReturnsCalendarEvents(
        Member member,
        CancellationToken cancellationToken
    )
    {
        var calendarEventsReadRepositoryMock = new Mock<ICalendarEventsReadRepository>();
        var eventFormats = new List<EventFormat> { EventFormat.Online };
        var calendarIds = new List<int> { 1 };
        var regionIds = new List<int> { 1 };
        var fromDate = DateTime.Today;
        var toDate = DateTime.Today.AddDays(7);
        var keyword = "test";
        var calendarEvent = new CalendarEventSummary();
        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(It.IsAny<GetCalendarEventsOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        var sut = new GetCalendarEventsQueryHandler(calendarEventsReadRepositoryMock.Object);
        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = member.Id,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            Page = 1,
            PageSize = 5
        };

        var result = await sut.Handle(query, cancellationToken);

        result.Result.CalendarEvents.Count.Should().Be(1);
        result.Result.TotalCount.Should().Be(0);

        var calendarEvents = result.Result.CalendarEvents;

        calendarEvents.First().Should().BeEquivalentTo(calendarEvent, options => options.Excluding(c => c.CalendarId).Excluding(c => c.RegionId).Excluding(c => c.TotalCount));
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_FromDateAfterToDate_ReturnsNoCalendarEvents_DoesNotCallRepository(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        GetCalendarEventsQueryHandler sut,
        CalendarEventSummary calendarEvent,
        Member member,
        List<EventFormat> eventFormats,
        List<int> calendarIds,
        List<int> regionIds,
        String keyword,
        CancellationToken cancellationToken
    )
    {
        var fromDate = DateTime.Today.AddDays(7);
        var toDate = DateTime.Today;

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(new GetCalendarEventsOptions
        {
            MemberId = member.Id,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            Page = 1,
            PageSize = 5
        }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = member.Id,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            Page = 1,
            PageSize = 5
        };
        var result = await sut.Handle(query, cancellationToken);

        result.Result.CalendarEvents.Count.Should().Be(0);
        result.Result.TotalCount.Should().Be(0);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(It.IsAny<GetCalendarEventsOptions>(), cancellationToken), Times.Never);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_FromDateNull_FromDateIsUpdatedToToday(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        GetCalendarEventsQueryHandler sut,
        CalendarEventSummary calendarEvent,
        Member member,
        List<EventFormat> eventFormats,
        List<int> calendarIds,
        List<int> regionIds,
        String keyword,
        CancellationToken cancellationToken
    )
    {
        DateTime? fromDate = null;
        var toDate = DateTime.Today;

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(new GetCalendarEventsOptions
        {
            MemberId = member.Id,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            Page = 1,
            PageSize = 5
        }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = member.Id,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            Page = 1,
            PageSize = 5
        };

        await sut.Handle(query, cancellationToken);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(It.IsAny<GetCalendarEventsOptions>(), cancellationToken), Times.Once);
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
        var calendarIds = new List<int>();
        var regionIds = new List<int>();
        var fromDate = DateTime.Today.AddDays(-7);
        var toDate = DateTime.Today;
        var keyword = string.Empty;
        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(new GetCalendarEventsOptions
        {
            MemberId = member.Id,
            FromDate = DateTime.Today,
            ToDate = toDate,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            Page = 1,
            PageSize = 5
        }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = member.Id,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Page = 1,
            PageSize = 5
        };
        await sut.Handle(query, cancellationToken);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(It.IsAny<GetCalendarEventsOptions>(), cancellationToken), Times.Once);
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
        var calendarIds = new List<int>();
        var regionIds = new List<int>();
        var fromDate = DateTime.Today;
        DateTime? toDate = null;
        var todayPlusOneYear = DateTime.Today.AddYears(1);
        var keyword = string.Empty;

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(new GetCalendarEventsOptions
        {
            MemberId = member.Id,
            FromDate = DateTime.Today,
            ToDate = todayPlusOneYear,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            Page = 1,
            PageSize = 5
        }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = member.Id,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            Page = 1,
            PageSize = 5
        };

        await sut.Handle(query, cancellationToken);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(It.IsAny<GetCalendarEventsOptions>(), cancellationToken), Times.Once);
    }
}