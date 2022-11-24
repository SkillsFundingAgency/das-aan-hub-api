
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.AAN.Application.Commands.PatchCalendarEvent;
using SFA.DAS.AAN.Hub.Api.Controllers;
using SFA.DAS.AAN.Application.UnitTests;


namespace SFA.DAS.AAN.Hub.Api.UnitTests.CreateCalendarEvent
{
    public class WhenPatchingCalendarEvent
    {
        private readonly Mock<IMediator> _mediator;
        private readonly CalendarController _controller;

        public WhenPatchingCalendarEvent()
        {
            _mediator = new Mock<IMediator>();
            _controller = new CalendarController(_mediator.Object, Mock.Of<ILogger<CalendarController>>());
        }

        [Theory, AutoMoqData]
        public async Task And_MediatorCommandSuccessful_Then_ReturnNoContentk(
            PatchCalendarEventCommand command,
            PatchCalendarEventResponse response
            )
        {
            response.warnings = null;
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            IActionResult result = await _controller.PatchCalendarEvent(1, Guid.NewGuid(), command);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NoContentResult>();
        }

        [Theory, AutoMoqData]
        public async Task And_MediatorCommandUnsuccessful_Then_ReturnOk(
            PatchCalendarEventCommand command,
            PatchCalendarEventResponse response
            )
        {
            response.warnings = new string[] { "test" };
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            IActionResult result = await _controller.PatchCalendarEvent(1, Guid.NewGuid(), command);

            result.Should().NotBeNull();

            object? model = ((OkObjectResult)result).Value;
            model.Should().NotBeNull();
            model.Should().BeAssignableTo<PatchCalendarEventResponse>();
            model.Should().BeEquivalentTo(response);
        }
    }
}
