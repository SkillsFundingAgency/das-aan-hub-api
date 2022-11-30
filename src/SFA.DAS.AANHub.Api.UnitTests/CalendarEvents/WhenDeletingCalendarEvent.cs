
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using SFA.DAS.AANHub.Application.Commands.DeleteCalendarEvent;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.UnitTests;


namespace SFA.DAS.AANHub.Api.UnitTests.DeleteCalendarEvent
{
    public class WhenDeletingCalendarEvent
    {
        private readonly Mock<IMediator> _mediator;
        private readonly CalendarController _controller;

        public WhenDeletingCalendarEvent()
        {
            _mediator = new Mock<IMediator>();
            _controller = new CalendarController(_mediator.Object, Mock.Of<ILogger<CalendarController>>());
        }

        [Test, AutoMoqData]
        public async Task And_MediatorCommandSuccessful_Then_ReturnNoContentk(
            DeleteCalendarEventCommand command,
            DeleteCalendarEventResponse response
            )
        {
            response.warnings = null;
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            IActionResult result = await _controller.DeleteCalendarEvent(1, Guid.NewGuid(), command);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NoContentResult>();
        }
    }
}
