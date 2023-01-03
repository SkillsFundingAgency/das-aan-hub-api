using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Queries.GetCalendarsForUser;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class WhenRequestCalendarsForMember
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<CalendarController>> _logger;

        public WhenRequestCalendarsForMember()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<CalendarController>>();
        }

        [Test]
        public async Task And_MediatorCommandSuccessful_Then_ReturnOk()
        {
            var guid = Guid.NewGuid();
            var query = new Mock<GetCalendarsForUserQuery>(guid);

            var response = new GetCalendarsForUserResult() { Permissions = new long[] { 1, 2 }, Calendars = new List<GetCalendarsForUserResultItem>() };
            _mediator.Setup(m => m.Send(It.IsAny<GetCalendarsForUserQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var controller = new CalendarController(_mediator.Object, _logger.Object);


            var result = await controller.GetCalendar(Guid.NewGuid());
            result.Should().NotBeNull();

            var model = ((OkObjectResult)result).Value;
            model.Should().NotBeNull();
            model.Should().BeAssignableTo<GetCalendarsForUserResult>();
            model.Should().BeEquivalentTo(response);
        }
    }
}
