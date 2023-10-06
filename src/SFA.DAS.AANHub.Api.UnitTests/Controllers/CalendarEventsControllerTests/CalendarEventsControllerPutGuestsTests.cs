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
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.EventGuests.PutEventGuests;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.CalendarEventsControllerTests;

public class CalendarEventsControllerPutGuestsTests
{
    [Test, AutoData]
    public async Task PutEventGuests_InvokesMediator(Guid requestedByMemberId, Guid calendarEventId, PutEventGuestsModel model, CancellationToken cancellationToken)
    {
        Mock<IMediator> mediatorMock = new();
        mediatorMock.Setup(m => m.Send(It.IsAny<PutEventGuestsCommand>(), cancellationToken)).ReturnsAsync(new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult()));
        CalendarEventsController sut = new(Mock.Of<ILogger<CalendarEventsController>>(), mediatorMock.Object);

        await sut.PutEventGuests(requestedByMemberId, calendarEventId, model, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<PutEventGuestsCommand>(c => c.CalendarEventId == calendarEventId && c.AdminMemberId == requestedByMemberId && c.Guests == model.Guests), cancellationToken));
    }

    [Test, AutoData]
    public async Task PutEventGuests_OnHandlerSuccess_ReturnsNoContentResponse(Guid requestedByMemberId, Guid calendarEventId, PutEventGuestsModel model, CancellationToken cancellationToken)
    {
        Mock<IMediator> mediatorMock = new();
        mediatorMock.Setup(m => m.Send(It.IsAny<PutEventGuestsCommand>(), cancellationToken)).ReturnsAsync(new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult()));
        CalendarEventsController sut = new(Mock.Of<ILogger<CalendarEventsController>>(), mediatorMock.Object);

        var result = await sut.PutEventGuests(requestedByMemberId, calendarEventId, model, cancellationToken);

        result.As<NoContentResult>().Should().NotBeNull();
    }

    [Test, AutoData]
    public async Task PutEventGuests_OnValidationErrors_ReturnsBadRequestResponse(Guid requestedByMemberId, Guid calendarEventId, PutEventGuestsModel model, List<ValidationFailure> errors, CancellationToken cancellationToken)
    {
        Mock<IMediator> mediatorMock = new();
        mediatorMock.Setup(m => m.Send(It.IsAny<PutEventGuestsCommand>(), cancellationToken)).ReturnsAsync(new ValidatedResponse<SuccessCommandResult>(errors));
        CalendarEventsController sut = new(Mock.Of<ILogger<CalendarEventsController>>(), mediatorMock.Object);

        var result = await sut.PutEventGuests(requestedByMemberId, calendarEventId, model, cancellationToken);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
    }
}
