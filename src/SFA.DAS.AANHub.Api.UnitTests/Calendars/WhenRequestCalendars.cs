using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Queries.GetCalendars;

namespace SFA.DAS.AANHub.Api.UnitTests.Calendars
{
    public class WhenRequestCalendars
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<CalendarController>> _logger;

        public WhenRequestCalendars()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<CalendarController>>();
        }

        [Test]
        public async Task And_MediatorCommandSuccessful_Then_ReturnOk()
        {
            IEnumerable<GetCalendarsResultItem> response = new List<GetCalendarsResultItem>();
            _mediator.Setup(m => m.Send(It.IsAny<IEnumerable<GetCalendarsResultItem>>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var controller = new CalendarController(_mediator.Object, _logger.Object);

            var result = await controller.GetCalendar(null);
            result.Should().NotBeNull();

            var model = ((OkObjectResult)result).Value;
            model.Should().NotBeNull();
            model.Should().BeAssignableTo<IEnumerable<GetCalendarsResultItem>>();
            model.Should().BeEquivalentTo(response);
        }
    }
}
