using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries;
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
       DateTime? startDate,
       DateTime? endDate,
       List<EventFormat> eventFormats,
       CancellationToken cancellationToken)
    {
        await sut.GetCalendarEvents(requestedByMemberId, startDate, endDate, eventFormats, cancellationToken);

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
        DateTime? startDate,
        DateTime? endDate,
        List<EventFormat> eventFormats,
        CancellationToken cancellationToken)
    {
        var emptyResponse = ValidatedResponse<GetCalendarEventsQueryResult>.EmptySuccessResponse();
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(emptyResponse);

        var result = await sut.GetCalendarEvents(requestedByMemberId, startDate, endDate, eventFormats, cancellationToken);
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        Guid requestedByMemberId,
        GetCalendarEventsQueryResult queryResult,
        DateTime? startDate,
        DateTime? endDate,
        List<EventFormat> eventFormats,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<GetCalendarEventsQueryResult>(queryResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

        var result = await sut.GetCalendarEvents(requestedByMemberId, startDate, endDate, eventFormats, cancellationToken);

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
        DateTime? startDate,
        DateTime? endDate,
        List<EventFormat> eventFormats,
        CancellationToken cancellationToken)
    {
        var errorResponse = new ValidatedResponse<GetCalendarEventsQueryResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(errorResponse);

        var result = await sut.GetCalendarEvents(requestedByMemberId, startDate, endDate, eventFormats, cancellationToken);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}