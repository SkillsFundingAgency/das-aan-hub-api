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
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class PartnersControllerTests
    {
        private readonly PartnersController _controller;
        private readonly Mock<IMediator> _mediator;

        public PartnersControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _controller = new PartnersController(Mock.Of<ILogger<PartnersController>>(), _mediator.Object);
        }

        [Test]
        [AutoMoqData]
        public async Task CreatePartners_InvokesRequest(
            CreatePartnerModel model,
            CreatePartnerMemberCommand command)
        {
            var response = new ValidatableResponse<CreatePartnerMemberCommandResponse>
            (new CreatePartnerMemberCommandResponse
            {
                MemberId = command.Id,
                Status = MembershipStatus.Live
            });

            model.Regions = new List<int>(new[]
            {
                1, 2
            });

            _mediator.Setup(m => m.Send(It.IsAny<CreatePartnerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreatePartner(Guid.NewGuid(), model);

            result.As<CreatedAtActionResult>().ControllerName.Should().Be("Partners");
            result.As<CreatedAtActionResult>().ActionName.Should().Be("CreatePartner");
            result.As<CreatedAtActionResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Test]
        [AutoMoqData]
        public async Task CreatePartner_InvokesRequest_WithErrors(
            CreatePartnerMemberCommand command)
        {
            var response = new ValidatableResponse<CreatePartnerMemberCommandResponse>(new CreatePartnerMemberCommandResponse
                {
                    MemberId = command.Id,
                    Status = MembershipStatus.Live
                },
                new List<string>
                {
                    new("Error")
                });

            var model = new CreatePartnerModel();
            _mediator.Setup(m => m.Send(It.IsAny<CreatePartnerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreatePartner(Guid.NewGuid(), model);

            result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Test]
        [AutoMoqData]
        public void CreatePartnerModel_And_Command_WithExpectedFields(
        )
        {
            const string email = "email@email.com";
            const string info = "ThisIsInformation";
            var date = DateTime.Now;
            const string name = "ThisIsAName";
            const string userName = "ThisIsAUserName";
            const string organisation = "ThisIsAnOrganisation";
            var regions = new List<int>(new[]
            {
                1, 2
            });

            var model = new CreatePartnerModel
            {
                Email = email,
                Information = info,
                Joined = date,
                Name = name,
                Regions = regions,
                UserName = userName,
                Organisation = organisation
            };

            var command = (CreatePartnerMemberCommand)model;
            var member = (Member)command;

            command.UserName.Should().Be(userName);
            command.Email.Should().Be(email);
            command.Organisation.Should().Be(organisation);
            command.Name.Should().Be(name);
            command.Information.Should().Be(info);
            command.Joined.Should().Be(date);
            command.Regions.Should().Equal(regions);

            member.UserType.Should().Be(MembershipUserType.Partner);
            member.Joined.Should().Be(date);
            member.Information.Should().Be(info);
            member.ReviewStatus.Should().Be(MembershipReviewStatus.New);
            member.Deleted.Should().BeNull();
            member.Status.Should().Be(MembershipStatus.Live);
            member?.Partner?.MemberId.Should().Be(member.Id);
            member?.Partner?.Email.Should().Be(email);
            member?.Partner?.UserName.Should().Be(userName);
            member?.Partner?.Organisation.Should().Be(organisation);
            member?.Partner?.IsActive.Should().Be(true);
        }
    }
}