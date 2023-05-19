using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.CalendarEventsControllerTests;

public class CalendarEventsControllerPutTests
{
    [Test]
    [MoqAutoData]
    public async Task Put_NewAttendanceNotCreated_Returns204NoContent(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(false));

        mediatorMock.Setup(m => m.Send(It.IsAny<PutAttendanceCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PutAttendance(Guid.NewGuid(), Guid.NewGuid(), true) as NoContentResult;

        result?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Test]
    [MoqAutoData]
    public async Task Put_NewAttendanceCreated_Returns201Created(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(true));

        mediatorMock.Setup(m => m.Send(It.IsAny<PutAttendanceCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PutAttendance(Guid.NewGuid(), Guid.NewGuid(), true) as NoContentResult;

        result?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Put_InvalidCalendarId_ReturnsAllErrors(
    [Frozen] Mock<IMediator> mediatorMock,
    [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepository,
    [Greedy] CalendarEventsController sut,
    [Frozen] List<ValidationFailure> errors)
    {
        calendarEventsReadRepository.Setup(c => c.GetCalendarEvent(It.IsAny<Guid>()))
                                    .ReturnsAsync(() => null);

        var response = new ValidatedResponse<SuccessCommandResult>(errors);

        mediatorMock.Setup(m => m.Send(It.IsAny<PutAttendanceCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PutAttendance(Guid.NewGuid(), Guid.NewGuid(), true) as BadRequestObjectResult;

        result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        result.Value.Should().BeEquivalentTo(errors.Select(e => new { e.ErrorMessage, e.PropertyName }));

    }
}
