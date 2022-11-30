
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using SFA.DAS.AANHub.Application.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.UnitTests;


namespace SFA.DAS.AANHub.Api.UnitTests.CreateCalendarEvent
{
    public class WhenPostingCreateCalendarEvent
    {
        private readonly Mock<IMediator> _mediator;
        private readonly CalendarController _controller;

        public WhenPostingCreateCalendarEvent()
        {
            _mediator = new Mock<IMediator>();
            _controller = new CalendarController(_mediator.Object, Mock.Of<ILogger<CalendarController>>());
        }

        [Test, AutoMoqData]
        public async Task And_MediatorCommandSuccessful_Then_ReturnOk(
            CreateCalendarEventCommand command,
            CreateCalendarEventResponse response
            )
        {
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            IActionResult result = await _controller.CreateCalendarEvent(1, command);

            result.Should().NotBeNull();

            object? model = ((OkObjectResult)result).Value;
            model.Should().NotBeNull();
            model.Should().BeAssignableTo<CreateCalendarEventResponse>();
            model.Should().BeEquivalentTo(response);
        }
    }
}
