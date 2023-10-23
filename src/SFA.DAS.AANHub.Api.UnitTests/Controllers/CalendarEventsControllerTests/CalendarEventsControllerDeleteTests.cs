using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.DeleteCalendarEvent;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.CalendarEventsControllerTests;
public class CalendarEventsControllerDeleteTests
{
    [Test, MoqAutoData]
    public async Task Put_DeleteSuccessful_Returns204NoContent(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        Guid calendarEventId,
        Guid memberId
        )
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());

        mediatorMock.Setup(m => m.Send(It.Is<DeleteCalendarEventCommand>(x => x.RequestedByMemberId == memberId && x.CalendarEventId == calendarEventId),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.Delete(calendarEventId, memberId) as NoContentResult;

        result?.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Delete_InvalidCalendarId_ReturnsAllErrors(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        [Frozen] List<ValidationFailure> errors)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(errors);

        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteCalendarEventCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.Delete(Guid.NewGuid(), Guid.NewGuid()) as BadRequestObjectResult;

        result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        result.Value.Should().BeEquivalentTo(errors.Select(e => new { e.ErrorMessage, e.PropertyName }));

    }
}
