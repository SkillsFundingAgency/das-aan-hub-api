
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.AAN.Application.Queries.GetCalendarEvents;
using SFA.DAS.AAN.Hub.Api.Controllers;


namespace SFA.DAS.AAN.Hub.Api.UnitTests.Calendars
{
    public class WhenRequestCalendarEvents
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<CalendarController>> _logger;

        public WhenRequestCalendarEvents()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<CalendarController>>();
        }

        [Fact]
        public async Task And_MediatorCommandSuccessful_Then_ReturnOk()
        {
            IEnumerable<GetCalendarEventsResultItem> response = new List<GetCalendarEventsResultItem>();
            _mediator.Setup(m => m.Send(It.IsAny<IEnumerable<GetCalendarEventsResultItem>>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            CalendarController controller = new CalendarController(_mediator.Object, _logger.Object);

            IActionResult result = await controller.GetCalendarEvents(2, Guid.NewGuid(), null, null, DateTime.Now, DateTime.Now);
            result.Should().NotBeNull();

            object? model = ((OkObjectResult)result).Value;
            model.Should().NotBeNull();
            model.Should().BeAssignableTo<IEnumerable<GetCalendarEventsResultItem>>();
            model.Should().BeEquivalentTo(response);
        }
    }
}
