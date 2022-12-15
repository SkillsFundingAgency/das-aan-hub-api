using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Queries.GetAllRegions;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class WhenRequestRegions
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<RegionController>> _logger;

        public WhenRequestRegions()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<RegionController>>();
        }

        [Test]
        public async Task And_MediatorCommandSuccessful_Then_ReturnOk()
        {
            var response = new GetAllRegionsResult();
            _mediator.Setup(m => m.Send(It.IsAny<GetAllRegionsResult>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var controller = new RegionController(_mediator.Object, _logger.Object);

            var result = await controller.GetListOfRegions() as OkObjectResult;

            result.Should().NotBeNull();
            var model = result?.Value;
            model.Should().BeEquivalentTo(response.Regions);

        }
    }
}
