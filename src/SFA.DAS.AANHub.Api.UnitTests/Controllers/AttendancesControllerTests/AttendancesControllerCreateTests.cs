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
using SFA.DAS.AANHub.Application.Attendances.Commands.CreateAttendance;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.AttendancesControllerTests;
public class AttendancesControllerCreateTests
{
    private readonly AttendancesController _controller;
    private readonly Mock<IMediator> _mediator;

    public AttendancesControllerCreateTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new AttendancesController(Mock.Of<ILogger<AttendancesController>>(), _mediator.Object);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task CreateAttendance_ReturnsExpectedResult(
        CreateAttendanceCommand command,
        [Frozen] CalendarEvent calendarEvent,
        [Frozen] Member memberEvent)
    {
        var response = new ValidatedResponse<CreateAttendanceCommandResponse>(new CreateAttendanceCommandResponse(command.Id));

        _mediator.Setup(m => m.Send(It.IsAny<CreateAttendanceCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var result = await _controller.CreateAttendance(calendarEvent.Id, memberEvent.Id) as CreatedAtActionResult;

        result?.ControllerName.Should().Be("Attendances");
        result?.ActionName.Should().Be("Get");
        result?.StatusCode.Should().Be(StatusCodes.Status201Created);
        result?.Value.As<CreateAttendanceCommandResponse>().AttendanceId.Should().Be(command.Id);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task CreateAttendance_UnknownCalendarEventId_ReturnsNotFound404(CalendarEvent calendarEvent, Member member)
    {
        var calendarEventsReadRepository = new Mock<ICalendarEventsReadRepository>();
        calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(calendarEvent.Id))
                                    .Returns<CalendarEvent>(null);
        var errorResponse = new ValidatedResponse<CreateAttendanceCommandResponse>
        (new List<ValidationFailure>
        {
            new("CalendarEventId", CreateAttendanceCommandValidator.EventNotFoundMessage)
        });

        var command = new CreateAttendanceCommand(calendarEvent.Id, member.Id);
        _mediator.Setup(m => m.Send(It.IsAny<CreateAttendanceCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);
        var result = await _controller.CreateAttendance(calendarEvent.Id, member.Id);

        result.As<NotFoundObjectResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [TestCase(CreateAttendanceCommandValidator.EventInPastMessage)]
    [TestCase(CreateAttendanceCommandValidator.EventCancelledMessage)]
    [TestCase(CreateAttendanceCommandValidator.EventIdNotSuppliedMessage)]
    public async Task CreateAttendance_CalendarEventCancelledInThePastOrNotSupplied_ReturnsBadRequest400(string errorMessage)
    {
        var calendarEvent = new CalendarEvent() { Id = Guid.NewGuid() };
        var member = new Member() { Id = Guid.NewGuid() };

        var calendarEventsReadRepository = new Mock<ICalendarEventsReadRepository>();
        calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(calendarEvent.Id))
                                    .ReturnsAsync(calendarEvent);

        var errorResponse = new ValidatedResponse<CreateAttendanceCommandResponse>
        (new List<ValidationFailure>
        {
            new("CalendarEventId", errorMessage)
        });

        var command = new CreateAttendanceCommand(calendarEvent.Id, member.Id);
        _mediator.Setup(m => m.Send(It.IsAny<CreateAttendanceCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);
        var result = await _controller.CreateAttendance(calendarEvent.Id, member.Id);

        result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [TestCase(RequestedByMemberIdValidator.RequestedByMemberHeaderEmptyErrorMessage)]
    [TestCase(RequestedByMemberIdValidator.RequestedByMemberIdNotFoundMessage)]
    public async Task CreateAttendance_RequestedByMemberIdValidationFailed_ReturnsBadRequest400(string errorMessage)
    {
        var calendarEvent = new CalendarEvent() { Id = Guid.NewGuid() };
        var member = new Member() { Id = Guid.NewGuid() };

        var calendarEventsReadRepository = new Mock<ICalendarEventsReadRepository>();
        calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(calendarEvent.Id))
                                    .ReturnsAsync(calendarEvent);

        var errorResponse = new ValidatedResponse<CreateAttendanceCommandResponse>(
            new List<ValidationFailure>
            {
                new("RequestedByMemberId", errorMessage)
            });

        var command = new CreateAttendanceCommand(calendarEvent.Id, member.Id);
        _mediator.Setup(m => m.Send(It.IsAny<CreateAttendanceCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);
        var result = await _controller.CreateAttendance(calendarEvent.Id, member.Id);

        result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}
