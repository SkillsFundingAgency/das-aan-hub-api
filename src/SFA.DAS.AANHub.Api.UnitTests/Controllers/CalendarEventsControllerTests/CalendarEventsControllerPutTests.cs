using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.CalendarEventsControllerTests;
public class CalendarEventsControllerPutTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly CancellationToken _cancellationToken = new();
    private CalendarEventsController _sut = new(Mock.Of<ILogger<CalendarEventsController>>(), Mock.Of<IMediator>(), Mock.Of<ICalendarEventsReadRepository>());

    [SetUp]
    public void Init()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<PutCalendarEventCommand>(), _cancellationToken)).ReturnsAsync(new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult()));
        _sut = new CalendarEventsController(Mock.Of<ILogger<CalendarEventsController>>(), _mediatorMock.Object, Mock.Of<ICalendarEventsReadRepository>());
    }

    [Test, AutoData]
    public async Task PutCalendarEvent_InvokesMediator(Guid requestedByMemberId, Guid calendarEventId, PutCalendarEventModel model)
    {
        await _sut.UpdateCalendarEvent(requestedByMemberId, calendarEventId, model, _cancellationToken);
        _mediatorMock.Verify(m => m.Send(It.Is<PutCalendarEventCommand>(c => c.CalendarEventId == calendarEventId && c.AdminMemberId == requestedByMemberId), _cancellationToken));
    }

    [Test, MoqAutoData]
    public async Task Put_CalendarEvent_Returns204NoContent(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        PutCalendarEventModel model,
        Guid requestedByMemberId,
        Guid calendarEventId)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(true));

        mediatorMock.Setup(m => m.Send(It.IsAny<PutCalendarEventCommand>(),
        It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await _sut.UpdateCalendarEvent(requestedByMemberId, calendarEventId, model, _cancellationToken) as NoContentResult;

        result?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Test, AutoData]
    public async Task PutCalendarEvent_OnValidationErrors_ReturnsBadRequestResponse(Guid requestedByMemberId, Guid calendarEventId, PutCalendarEventModel model, List<ValidationFailure> errors)
    {
        _sut = new CalendarEventsController(Mock.Of<ILogger<CalendarEventsController>>(), _mediatorMock.Object, Mock.Of<ICalendarEventsReadRepository>());

        _mediatorMock.Setup(m => m.Send(It.IsAny<PutCalendarEventCommand>(), _cancellationToken)).ReturnsAsync(new ValidatedResponse<SuccessCommandResult>(errors));
        _sut = new(Mock.Of<ILogger<CalendarEventsController>>(), _mediatorMock.Object, Mock.Of<ICalendarEventsReadRepository>());
        var result = await _sut.UpdateCalendarEvent(requestedByMemberId, calendarEventId, model, _cancellationToken);
        result.As<BadRequestObjectResult>().Should().NotBeNull();
    }
}
