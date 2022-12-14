using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Commands.CreateMember;
using SFA.DAS.AANHub.Application.Responses;
using SFA.DAS.AANHub.Application.UnitTests;
using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class WhenPostingCreateMember
    {
        private readonly Mock<IMediator> _mediator;
        private readonly MemberController _controller;

        public WhenPostingCreateMember()
        {
            _mediator = new Mock<IMediator>();
            _controller = new MemberController(_mediator.Object, Mock.Of<ILogger<MemberController>>());
        }

        [Test, AutoMoqData]
        public async Task And_MediatorApprenticeCommandSuccessful_Then_ReturnOk(
            CreateMemberCommand command,
            CreateMemberResponse response
            )
        {
            command.UserType = MembershipUserType.Apprentice;
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreateApprenticeMember(command);
            ApplyTests(result, response);
        }

        [Test, AutoMoqData]
        public async Task And_MediatorPartnerCommandSuccessful_Then_ReturnOk(
            CreateMemberCommand command,
            CreateMemberResponse response
            )
        {
            command.UserType = MembershipUserType.Partner;
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreatePartnerMember(command);
            ApplyTests(result, response);
        }

        [Test, AutoMoqData]
        public async Task And_MediatorAdminCommandSuccessful_Then_ReturnOk(
            CreateMemberCommand command,
            CreateMemberResponse response
            )
        {
            var result = await ExecuteMediatorCommand(command, response, MembershipUserType.Admin);
            ApplyTests(result, response);
        }

        private async Task<IActionResult> ExecuteMediatorCommand(
            CreateMemberCommand command,
            CreateMemberResponse response,
            MembershipUserType userType
            )
        {
            command.UserType = userType;
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            return await _controller.CreateAdminMember(command);
        }

        private static void ApplyTests(IActionResult result, CreateMemberResponse response)
        {
            result.Should().NotBeNull();

            var model = ((OkObjectResult)result).Value;
            model.Should().NotBeNull();
            model.Should().BeAssignableTo<CreateMemberApiResponse>();
            model.Should().BeEquivalentTo(new CreateMemberApiResponse(response));
        }
    }
}
