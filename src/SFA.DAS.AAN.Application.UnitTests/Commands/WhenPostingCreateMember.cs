
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.AAN.Application.Commands.CreateMember;
using SFA.DAS.AAN.Hub.Api.Controllers;
using SFA.DAS.AAN.Application.ApiResponses;
using SFA.DAS.AAN.Application.UnitTests;
using SFA.DAS.AAN.Domain.Enums;


namespace SFA.DAS.AAN.Hub.Api.UnitTests.Members
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

        [Theory, AutoMoqData]
        public async Task And_MediatorApprenticeCommandSuccessful_Then_ReturnOk(
            CreateMemberCommand command,
            CreateMemberResponse response
            )
        {
            IActionResult result = await ExecuteMediatorCommand(command, response, MembershipUserTypes.Apprentice);
            await ApplyTests(result, response);
        }

        [Theory, AutoMoqData]
        public async Task And_MediatorEmployerCommandSuccessful_Then_ReturnOk(
            CreateMemberCommand command,
            CreateMemberResponse response
            )
        {
            IActionResult result = await ExecuteMediatorCommand(command, response, MembershipUserTypes.Employer);
            await ApplyTests(result, response);
        }

        [Theory, AutoMoqData]
        public async Task And_MediatorPartnerCommandSuccessful_Then_ReturnOk(
            CreateMemberCommand command,
            CreateMemberResponse response
            )
        {
            IActionResult result = await ExecuteMediatorCommand(command, response, MembershipUserTypes.Partner);
            await ApplyTests(result, response);
        }

        [Theory, AutoMoqData]
        public async Task And_MediatorAdminCommandSuccessful_Then_ReturnOk(
            CreateMemberCommand command,
            CreateMemberResponse response
            )
        {
            IActionResult result = await ExecuteMediatorCommand(command, response, MembershipUserTypes.Admin);
            await ApplyTests(result, response);
        }




        private async Task<IActionResult> ExecuteMediatorCommand(
            CreateMemberCommand command,
            CreateMemberResponse response,
            MembershipUserTypes userType
            )
        {
            command.UserType = userType;
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            return await _controller.CreateAdminMember(command);
        }

        private async Task ApplyTests(IActionResult result, CreateMemberResponse response)
        {
            result.Should().NotBeNull();

            object? model = ((OkObjectResult)result).Value;
            model.Should().NotBeNull();
            model.Should().BeAssignableTo<CreateMemberApiResponse>();
            model.Should().BeEquivalentTo(new CreateMemberApiResponse(response));
        }
    }
}
