using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.CalendarEventsControllerTests;

public class CalendarEventsControllerGetTests
{
    [Test]
    [MoqAutoData]
    public async Task GetCalendarEventById_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] CalendarEventsController sut,
       Guid calendarEventId,
       Guid requestedByUserId)
    {
        await sut.Get(calendarEventId,requestedByUserId);

        mediatorMock.Verify(m => m.Send(It.Is<GetCalendarEventByIdQuery>(q => q.CalendarEventId == calendarEventId), It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task GetCalendarEventById_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        Guid calendarEventsId,
        Guid requestedByUserId)
    {
        var notFoundResponse = ValidatedResponse<GetCalendarEventByIdQueryResult>.EmptySuccessResponse();
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventByIdQuery>(q => q.CalendarEventId == calendarEventsId), It.IsAny<CancellationToken>())).ReturnsAsync(notFoundResponse);

        var result = await sut.Get(calendarEventsId, requestedByUserId);

        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task GetCalendarEventById_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        Guid calendarEventId,
        Guid requestedByUserId,
        GetCalendarEventByIdQueryResult queryResult)
    {
        var response = new ValidatedResponse<GetCalendarEventByIdQueryResult>(queryResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventByIdQuery>(q => q.CalendarEventId == calendarEventId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

        var result = await sut.Get(calendarEventId, requestedByUserId);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }

    [Test]
    [MoqAutoData]
    public async Task GetCalendarEventById_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        List<ValidationFailure> errors,
        Guid calendarEventId,
        Guid requestedByUserId)
    {
        var errorResponse = new ValidatedResponse<GetCalendarEventByIdQueryResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventByIdQuery>(q => q.CalendarEventId == calendarEventId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(errorResponse);

        var result = await sut.Get(calendarEventId, requestedByUserId);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
