
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.AAN.Application.Commands.DeleteCalendarEvent;
using SFA.DAS.AAN.Hub.Api.Controllers;
using SFA.DAS.AAN.Application.UnitTests;


namespace SFA.DAS.AAN.Hub.Api.UnitTests.DeleteCalendarEvent
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

        [Theory, AutoMoqData]
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
