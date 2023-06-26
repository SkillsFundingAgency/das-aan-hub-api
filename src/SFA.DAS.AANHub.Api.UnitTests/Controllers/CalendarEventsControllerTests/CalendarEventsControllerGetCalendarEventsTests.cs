using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
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
       CancellationToken cancellationToken)
    {
        await sut.GetCalendarEvents(requestedByMemberId, fromDate, toDate, eventFormats, calendarIds, regionIds, cancellationToken);

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
        CancellationToken cancellationToken)
    {
        var emptyResponse = ValidatedResponse<GetCalendarEventsQueryResult>.EmptySuccessResponse();
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(emptyResponse);

        var result = await sut.GetCalendarEvents(requestedByMemberId, fromDate, toDate, eventFormats, calendarIds, regionIds, cancellationToken);
        result.As<NotFoundResult>().Should().NotBeNull();
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
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<GetCalendarEventsQueryResult>(queryResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

        var result = await sut.GetCalendarEvents(requestedByMemberId, fromDate, toDate, eventFormats, calendarIds, regionIds, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }

    [Test]
    [MoqAutoData]
    public async Task Get_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        List<ValidationFailure> errors,
        Guid requestedByMemberId,
        DateTime? fromDate,
        DateTime? toDate,
        List<EventFormat> eventFormats,
        List<int> calendarIds,
        List<int> regionIds,
        CancellationToken cancellationToken)
    {
        var errorResponse = new ValidatedResponse<GetCalendarEventsQueryResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(errorResponse);

        var result = await sut.GetCalendarEvents(requestedByMemberId, fromDate, toDate, eventFormats, calendarIds, regionIds, cancellationToken);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
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
        var sut = new CalendarEventsController(loggerMock.Object, mediatorMock.Object);
        var result = new ValidatedResponse<GetCalendarEventsQueryResult>();

        mediatorMock.Setup(x => x.Send(It.IsAny<GetCalendarEventsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);


        await sut.GetCalendarEvents(Guid.NewGuid(), DateTime.Today, DateTime.Today, new List<EventFormat>(),
            new List<int>(), new List<int>(), new CancellationToken(), page, pageSize);

        mediatorMock.Verify(
            m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.Page == expectedPage && q.PageSize == expectedPageSize),
                It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Get_DefaultsSetsPageAndPageSizeAsExpected()
    {
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<CalendarEventsController>>();
        var sut = new CalendarEventsController(loggerMock.Object, mediatorMock.Object);
        var result = new ValidatedResponse<GetCalendarEventsQueryResult>();
        var defaultPage = 1;
        mediatorMock.Setup(x => x.Send(It.IsAny<GetCalendarEventsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);


        await sut.GetCalendarEvents(Guid.NewGuid(), DateTime.Today, DateTime.Today, new List<EventFormat>(),
            new List<int>(), new List<int>(), new CancellationToken());

        mediatorMock.Verify(
            m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.Page == defaultPage && q.PageSize == Constants.CalendarEvents.PageSize),
                It.IsAny<CancellationToken>()));
    }
}