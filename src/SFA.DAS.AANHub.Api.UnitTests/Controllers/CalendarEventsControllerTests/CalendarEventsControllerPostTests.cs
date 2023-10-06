using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.CalendarEventsControllerTests;

public class CalendarEventsControllerPostTests
{
    [Test, AutoData]
    public async Task CreateCalendarEvent_InvokesCommand(Guid requestedByMemberId, CreateCalendarEventModel model, CancellationToken cancellationToken)
    {
        Mock<IMediator> mediatorMock = new();
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateCalendarEventCommand>(), cancellationToken)).ReturnsAsync(new ValidatedResponse<CreateCalendarEventCommandResult>(new CreateCalendarEventCommandResult(requestedByMemberId)));
        CalendarEventsController sut = new(Mock.Of<ILogger<CalendarEventsController>>(), mediatorMock.Object, Mock.Of<ICalendarEventsReadRepository>());

        await sut.CreateCalendarEvent(requestedByMemberId, model, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<CreateCalendarEventCommand>(c => c.AdminMemberId == requestedByMemberId), cancellationToken));
    }

    [Test, AutoData]
    public async Task CreateCalendarEvent_ReturnsCreatedResult(Guid requestedByMemberId, CreateCalendarEventModel model, Guid calendarEventId, CancellationToken cancellationToken)
    {
        Mock<IMediator> mediatorMock = new();
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateCalendarEventCommand>(), cancellationToken)).ReturnsAsync(new ValidatedResponse<CreateCalendarEventCommandResult>(new CreateCalendarEventCommandResult(calendarEventId)));
        CalendarEventsController sut = new(Mock.Of<ILogger<CalendarEventsController>>(), mediatorMock.Object, Mock.Of<ICalendarEventsReadRepository>());

        var result = await sut.CreateCalendarEvent(requestedByMemberId, model, cancellationToken);

        result.As<CreatedAtActionResult>().Should().NotBeNull();
        result.As<CreatedAtActionResult>().ControllerName.Should().Be(sut.ControllerName);
        result.As<CreatedAtActionResult>().ActionName.Should().Be("Get");
        result.As<CreatedAtActionResult>().Value.As<CreateCalendarEventCommandResult>().CalendarEventId.Should().Be(calendarEventId);
    }

    [Test, AutoData]
    public async Task CreateCalendarEvent_ReturnsBadRequestResult(Guid requestedByMemberId, CreateCalendarEventModel model, CancellationToken cancellationToken)
    {
        var errorResponse = new ValidatedResponse<CreateCalendarEventCommandResult>
        (new List<ValidationFailure>
        {
            new("Name", "error")
        });
        Mock<IMediator> mediatorMock = new();
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateCalendarEventCommand>(), cancellationToken)).ReturnsAsync(errorResponse);
        CalendarEventsController sut = new(Mock.Of<ILogger<CalendarEventsController>>(), mediatorMock.Object, Mock.Of<ICalendarEventsReadRepository>());

        var result = await sut.CreateCalendarEvent(requestedByMemberId, model, cancellationToken);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
    }
}
