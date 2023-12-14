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
        bool showUserEventsOnly,
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
            ShowUserEventsOnly = showUserEventsOnly,
            IsActive = null,
            Page = 1,
            PageSize = 5
        };

        var result = await sut.Handle(query, cancellationToken);

        result.CalendarEvents.Count.Should().Be(0);
        result.TotalCount.Should().Be(0);
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
        var showUserEventsOnly = true;
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
            ShowUserEventsOnly = showUserEventsOnly,
            IsActive = true,
            Page = 1,
            PageSize = 5
        };

        var result = await sut.Handle(query, cancellationToken);

        result.CalendarEvents.Count.Should().Be(1);
        result.TotalCount.Should().Be(0);

        var calendarEvents = result.CalendarEvents;

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
        bool? isActive,
        bool showUserEventsOnly,
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
            IsActive = isActive,
            ShowUserEventsOnly = showUserEventsOnly,
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
            ShowUserEventsOnly = showUserEventsOnly,
            IsActive = isActive,
            Page = 1,
            PageSize = 5
        };
        var result = await sut.Handle(query, cancellationToken);

        result.CalendarEvents.Count.Should().Be(0);
        result.TotalCount.Should().Be(0);
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
        Boolean? isActive,
        Boolean showUserEventsOnly,
        CancellationToken cancellationToken
    )
    {
        DateTime? fromDate = null;
        var toDate = DateTime.Today.AddDays(1);

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(new GetCalendarEventsOptions
        {
            MemberId = member.Id,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            IsActive = isActive,
            ShowUserEventsOnly = showUserEventsOnly,
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
            IsActive = isActive,
            ShowUserEventsOnly = showUserEventsOnly,
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
        var toDate = DateTime.Today.AddDays(1);
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
            IsActive = null,
            ShowUserEventsOnly = false,
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
            IsActive = null,
            ShowUserEventsOnly = false,
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
        bool? isActive = null;
        bool showUserEventsOnly = false;

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(new GetCalendarEventsOptions
        {
            MemberId = member.Id,
            FromDate = DateTime.Today,
            ToDate = todayPlusOneYear,
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            IsActive = isActive,
            ShowUserEventsOnly = showUserEventsOnly,
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
            IsActive = isActive,
            ShowUserEventsOnly = showUserEventsOnly,
            Page = 1,
            PageSize = 5
        };

        await sut.Handle(query, cancellationToken);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(It.IsAny<GetCalendarEventsOptions>(), cancellationToken), Times.Once);
    }

    [TestCase(null, null, 0)]
    [TestCase("", null, 0)]
    [TestCase("event", "event", 1)]
    [TestCase("west event", "west event", 2)]
    [TestCase("north-west event", "north west event", 3)]
    [TestCase("1 event", "1 event", 2)]
    [TestCase("'--;<>/**/_1 event 2 3", "1 event 2 3", 4)]
    public async Task Handle_Keyword_CheckUsedKeywordExpected(string? keyword, string? expectedKeywordUsed, int keywordCount)
    {
        var calendarEventsReadRepositoryMock = new Mock<ICalendarEventsReadRepository>();
        var cancellationToken = new CancellationToken();
        var calendarEvent = new CalendarEventSummary();
        var memberId = Guid.NewGuid();
        var eventFormats = new List<EventFormat>();
        var calendarIds = new List<int>();
        var regionIds = new List<int>();

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(It.IsAny<GetCalendarEventsOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = memberId,
            FromDate = DateTime.Now,
            ToDate = DateTime.Today.AddDays(1),
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            ShowUserEventsOnly = true,
            Page = 1,
            PageSize = 5
        };
        var sut = new GetCalendarEventsQueryHandler(calendarEventsReadRepositoryMock.Object);

        await sut.Handle(query, cancellationToken);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(
            It.Is<GetCalendarEventsOptions>(c => c.Keyword == expectedKeywordUsed && c.KeywordCount == keywordCount && c.ShowUserEventsOnly == true), cancellationToken), Times.Once);
    }

    [TestCase(null)]
    [TestCase(true)]
    [TestCase(false)]
    public async Task Handle_IsActive_CheckIsActiveExpected(bool? isActive)
    {
        var calendarEventsReadRepositoryMock = new Mock<ICalendarEventsReadRepository>();
        var cancellationToken = new CancellationToken();
        var calendarEvent = new CalendarEventSummary();
        var memberId = Guid.NewGuid();
        var eventFormats = new List<EventFormat>();
        var calendarIds = new List<int>();
        var regionIds = new List<int>();
        var keyword = "test";
        var showUserEventsOnly = true;

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(It.IsAny<GetCalendarEventsOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = memberId,
            FromDate = DateTime.Now,
            ToDate = DateTime.Today.AddDays(1),
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            IsActive = isActive,
            ShowUserEventsOnly = showUserEventsOnly,
            Page = 1,
            PageSize = 5
        };
        var sut = new GetCalendarEventsQueryHandler(calendarEventsReadRepositoryMock.Object);

        await sut.Handle(query, cancellationToken);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(
            It.Is<GetCalendarEventsOptions>(c => c.IsActive == isActive), cancellationToken), Times.Once);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Handle_ShowUserEventsOnly_CheckAsExpected(bool showUserEventsOnly)
    {
        var calendarEventsReadRepositoryMock = new Mock<ICalendarEventsReadRepository>();
        var cancellationToken = new CancellationToken();
        var calendarEvent = new CalendarEventSummary();
        var memberId = Guid.NewGuid();
        var eventFormats = new List<EventFormat>();
        var calendarIds = new List<int>();
        var regionIds = new List<int>();
        var keyword = "test";

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(It.IsAny<GetCalendarEventsOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CalendarEventSummary> { calendarEvent });

        var query = new GetCalendarEventsQuery
        {
            RequestedByMemberId = memberId,
            FromDate = DateTime.Now,
            ToDate = DateTime.Today.AddDays(1),
            EventFormats = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Keyword = keyword,
            IsActive = true,
            ShowUserEventsOnly = showUserEventsOnly,
            Page = 1,
            PageSize = 5
        };
        var sut = new GetCalendarEventsQueryHandler(calendarEventsReadRepositoryMock.Object);

        await sut.Handle(query, cancellationToken);
        calendarEventsReadRepositoryMock.Verify(x => x.GetCalendarEvents(
            It.Is<GetCalendarEventsOptions>(c => c.ShowUserEventsOnly == showUserEventsOnly), cancellationToken), Times.Once);
    }
}