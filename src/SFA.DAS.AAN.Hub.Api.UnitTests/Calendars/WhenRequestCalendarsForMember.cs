
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.AAN.Application.Queries.GetCalendarsForUser;
using SFA.DAS.AAN.Hub.Api.Controllers;


namespace SFA.DAS.AAN.Hub.Api.UnitTests.Calendars
{
    public class WhenRequestCalendarsForMember
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<CalendarController>> _logger;
        //private readonly Mock<GetCalendarsForUserQuery> _query;

        public WhenRequestCalendarsForMember()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<CalendarController>>();
        }

        [Fact]
        public async Task And_MediatorCommandSuccessful_Then_ReturnOk()
        {
            Guid guid = Guid.NewGuid();
            var query = new Mock<GetCalendarsForUserQuery>(guid);

            GetCalendarsForUserResult response = new GetCalendarsForUserResult() { Permissions = new long[] { 1, 2 }, Calendars = new List<GetCalendarsForUserResultItem>() };
            _mediator.Setup(m => m.Send(It.IsAny<GetCalendarsForUserQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            CalendarController controller = new CalendarController(_mediator.Object, _logger.Object);


            IActionResult result = await controller.GetCalendar(Guid.NewGuid());
            result.Should().NotBeNull();

            object? model = ((OkObjectResult)result).Value;
            model.Should().NotBeNull();
            model.Should().BeAssignableTo<GetCalendarsForUserResult>();
            model.Should().BeEquivalentTo(response);
        }
    }
}
