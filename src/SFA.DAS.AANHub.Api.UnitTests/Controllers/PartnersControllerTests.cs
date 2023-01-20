using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Partners;
using SFA.DAS.AANHub.Application.UnitTests;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class PartnersControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly PartnersController _controller;
        public PartnersControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _controller = new PartnersController(Mock.Of<ILogger<PartnersController>>(), _mediator.Object);

        }
        [Test, AutoMoqData]
        public async Task CreatePartners_InvokesRequest(
            CreatePartnerModel model,
            CreatePartnerMemberCommand command)
        {
            var response = new ValidatableResponse<CreatePartnerMemberCommandResponse>
            (new CreatePartnerMemberCommandResponse()
            {
                MemberId = command.Id,
                Status = MembershipStatus.Live,
            });

            model.Regions = new List<int>(new[] { 1, 2, });
            _mediator.Setup(m => m.Send(It.IsAny<CreatePartnerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreatePartner(Guid.NewGuid(), model);

            result.As<CreatedAtActionResult>().ControllerName.Should().Be("Partners");
            result.As<CreatedAtActionResult>().ActionName.Should().Be("CreatePartner");
            result.As<CreatedAtActionResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Test, AutoMoqData]
        public async Task CreatePartner_InvokesRequest_WithErrors(
            CreatePartnerMemberCommand command)
        {
            var response = new ValidatableResponse<CreatePartnerMemberCommandResponse>
            (new CreatePartnerMemberCommandResponse
            {
                MemberId = command.Id,
                Status = MembershipStatus.Live
            }, new List<string> { new("Error") });
            var model = new CreatePartnerModel();
            _mediator.Setup(m => m.Send(It.IsAny<CreatePartnerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreatePartner(Guid.NewGuid(), model);

            result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
