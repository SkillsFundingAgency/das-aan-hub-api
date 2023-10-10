using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.CalendarEventsControllerTests;
public class CalendarEventsControllerGetCalendarEventsTests
{
    [Test]
    [MoqAutoData]
    public async Task Get_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] CalendarEventsController sut,
       Guid requestedByMemberId,
       DateTime? fromDate,
       DateTime? toDate,
       List<EventFormat> eventFormats,
       List<int> calendarIds,
       List<int> regionIds,
       string keyword,
       bool? isActive,
       CancellationToken cancellationToken)
    {
        var getCalendarEventsModel = new GetCalendarEventsModel
        {
            RequestedByMemberId = requestedByMemberId,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = calendarIds,
            RegionId = regionIds,
            Keyword = keyword,
            IsActive = isActive
        };

        await sut.GetCalendarEvents(getCalendarEventsModel, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task Get_HandlerReturnsNullResult_ReturnsEmptyResultResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        Guid requestedByMemberId,
        DateTime? fromDate,
        DateTime? toDate,
        List<EventFormat> eventFormats,
        List<int> calendarIds,
        List<int> regionIds,
        string keyword,
        bool? isActive,
        CancellationToken cancellationToken)
    {
        var emptyResponse = (GetCalendarEventsQueryResult?)null;
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))!
                    .ReturnsAsync(emptyResponse);

        var getCalendarEventsModel = new GetCalendarEventsModel
        {
            RequestedByMemberId = requestedByMemberId,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = calendarIds,
            RegionId = regionIds,
            Keyword = keyword,
            IsActive = isActive
        };

        var result = await sut.GetCalendarEvents(getCalendarEventsModel, cancellationToken);
        result.As<OkObjectResult>().Should().NotBeNull();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        Guid requestedByMemberId,
        GetCalendarEventsQueryResult queryResult,
        DateTime? fromDate,
        DateTime? toDate,
        List<EventFormat> eventFormats,
        List<int> calendarIds,
        List<int> regionIds,
        string keyword,
        bool? isActive,
        CancellationToken cancellationToken)
    {

        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(queryResult);

        var getCalendarEventsModel = new GetCalendarEventsModel
        {
            RequestedByMemberId = requestedByMemberId,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = calendarIds,
            RegionId = regionIds,
            Keyword = keyword,
            IsActive = isActive
        };

        var result = await sut.GetCalendarEvents(getCalendarEventsModel, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }

    [TestCase(1, 5, 1, 5)]
    [TestCase(0, 5, 1, 5)]
    [TestCase(-1, 5, 1, 5)]
    [TestCase(0, 5, 1, 5)]
    [TestCase(1, 6, 1, 6)]
    [TestCase(2, 6, 2, 6)]
    [TestCase(1, 0, 1, 5)]
    [TestCase(1, -1, 1, 5)]
    public async Task Get_SetsPageAndPageSizeAsExpected(int page, int pageSize, int expectedPage, int expectedPageSize)
    {
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<CalendarEventsController>>();
        var sut = new CalendarEventsController(loggerMock.Object, mediatorMock.Object, Mock.Of<ICalendarEventsReadRepository>());
        var result = new GetCalendarEventsQueryResult();

        mediatorMock.Setup(x => x.Send(It.IsAny<GetCalendarEventsQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(result);

        var getCalendarEventsModel = new GetCalendarEventsModel
        {
            RequestedByMemberId = Guid.NewGuid(),
            FromDate = DateTime.Today,
            ToDate = DateTime.Today,
            EventFormat = new List<EventFormat>(),
            CalendarId = new List<int>(),
            RegionId = new List<int>(),
            Keyword = string.Empty,
            IsActive = null,
            Page = page,
            PageSize = pageSize
        };

        await sut.GetCalendarEvents(getCalendarEventsModel, new CancellationToken());

        mediatorMock.Verify(
            m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.Page == expectedPage && q.PageSize == expectedPageSize),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Get_DefaultsSetsPageAndPageSizeAsExpected()
    {
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<CalendarEventsController>>();
        var sut = new CalendarEventsController(loggerMock.Object, mediatorMock.Object, Mock.Of<ICalendarEventsReadRepository>());
        var result = new GetCalendarEventsQueryResult();
        var defaultPage = 1;
        mediatorMock.Setup(x => x.Send(It.IsAny<GetCalendarEventsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var getCalendarEventsModel = new GetCalendarEventsModel
        {
            RequestedByMemberId = Guid.NewGuid(),
            FromDate = DateTime.Today,
            ToDate = DateTime.Today,
            EventFormat = new List<EventFormat>(),
            CalendarId = new List<int>(),
            RegionId = new List<int>(),
            Keyword = string.Empty,
            IsActive = null
        };

        await sut.GetCalendarEvents(getCalendarEventsModel, new CancellationToken());

        mediatorMock.Verify(
            m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.Page == defaultPage && q.PageSize == Constants.CalendarEvents.PageSize),
                It.IsAny<CancellationToken>()));
    }
}