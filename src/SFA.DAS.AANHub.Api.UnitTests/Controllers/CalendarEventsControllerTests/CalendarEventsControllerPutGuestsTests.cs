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
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.CalendarEventsControllerTests;

public class CalendarEventsControllerPutGuestsTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly CancellationToken _cancellationToken = new();
    private CalendarEventsController _sut = new(Mock.Of<ILogger<CalendarEventsController>>(), Mock.Of<IMediator>(), Mock.Of<ICalendarEventsReadRepository>());

    [SetUp]
    public void Init()
    {
        _mediatorMock.Setup(m => m.Send(It.IsAny<PutEventGuestsCommand>(), _cancellationToken)).ReturnsAsync(new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult()));
        _sut = new CalendarEventsController(Mock.Of<ILogger<CalendarEventsController>>(), _mediatorMock.Object, Mock.Of<ICalendarEventsReadRepository>());
    }

    [Test, AutoData]
    public async Task PutEventGuests_InvokesMediator(Guid requestedByMemberId, Guid calendarEventId, PutEventGuestsModel model)
    {
        await _sut.PutEventGuests(requestedByMemberId, calendarEventId, model, _cancellationToken);
        _mediatorMock.Verify(m => m.Send(It.Is<PutEventGuestsCommand>(c => c.CalendarEventId == calendarEventId && c.AdminMemberId == requestedByMemberId && c.Guests == model.Guests), _cancellationToken));
    }

    [Test, AutoData]
    public async Task PutEventGuests_OnHandlerSuccess_ReturnsNoContentResponse(Guid requestedByMemberId, Guid calendarEventId, PutEventGuestsModel model)
    {
        var result = await _sut.PutEventGuests(requestedByMemberId, calendarEventId, model, _cancellationToken);
        result.As<NoContentResult>().Should().NotBeNull();
    }

    [Test, AutoData]
    public async Task PutEventGuests_OnValidationErrors_ReturnsBadRequestResponse(Guid requestedByMemberId, Guid calendarEventId, PutEventGuestsModel model, List<ValidationFailure> errors)
    {
        _sut = new CalendarEventsController(Mock.Of<ILogger<CalendarEventsController>>(), _mediatorMock.Object, Mock.Of<ICalendarEventsReadRepository>());

        _mediatorMock.Setup(m => m.Send(It.IsAny<PutEventGuestsCommand>(), _cancellationToken)).ReturnsAsync(new ValidatedResponse<SuccessCommandResult>(errors));
        _sut = new(Mock.Of<ILogger<CalendarEventsController>>(), _mediatorMock.Object, Mock.Of<ICalendarEventsReadRepository>());
        var result = await _sut.PutEventGuests(requestedByMemberId, calendarEventId, model, _cancellationToken);
        result.As<BadRequestObjectResult>().Should().NotBeNull();
    }
}
