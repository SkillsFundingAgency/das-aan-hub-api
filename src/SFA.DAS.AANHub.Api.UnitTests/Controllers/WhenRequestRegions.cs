using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Regions.Queries.GetRegions;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class WhenRequestRegions
    {
        private readonly Mock<ILogger<RegionsController>> _logger;
        private readonly Mock<IMediator> _mediator;

        public WhenRequestRegions()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<RegionsController>>();
        }

        [Test]
        public async Task And_MediatorCommandSuccessful_Then_ReturnOk()
        {
            var response = new ValidatedResponse<GetRegionsQueryResult>(new GetRegionsQueryResult
            {
                Regions = new List<Region>()
            });

            _mediator.Setup(m => m.Send(It.IsAny<GetRegionsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var controller = new RegionsController(_mediator.Object, _logger.Object);

            var result = await controller.GetRegions() as OkObjectResult;

            result.Should().NotBeNull();

            var model = result?.Value;

            model.Should().BeEquivalentTo(response.Result);
        }
    }
}