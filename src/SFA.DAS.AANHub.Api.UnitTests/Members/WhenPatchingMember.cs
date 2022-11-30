using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Commands.ModifyMember;
using SFA.DAS.AANHub.Application.UnitTests;

namespace SFA.DAS.AANHub.Api.UnitTests.Members
{
    public class WhenPatchingMember
    {
        private readonly Mock<IMediator> _mediator;
        private readonly MemberController _controller;
        private readonly string _memberId = Guid.NewGuid().ToString();
        public WhenPatchingMember()
        {
            _mediator = new Mock<IMediator>();
            _controller = new MemberController(_mediator.Object, Mock.Of<ILogger<MemberController>>());
        }

        [Test, AutoMoqData]
        public async Task And_MediatorCommandSuccessful_Then_ReturnOk(
            ModifyMemberCommand command,
            ModifyMemberResponse response
        )
        {
            command.UserId = _memberId;
            var result = await ExecuteMediatorCommand(command, response, _memberId);

            result.Should().NotBeNull();
            var model = ((OkObjectResult)result);
            model.StatusCode.Should().Be(200);
            model.Value.Should().BeNull();
        }
        [Test, AutoMoqData]
        public async Task And_MemberIdIsEmptyOrNull_Then_ReturnBadRequest(
            ModifyMemberCommand command,
            ModifyMemberResponse response
        )
        {
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.ManageMembership(command, string.Empty);

            result.Should().NotBeNull();
            var model = ((BadRequestObjectResult)result);
            model.StatusCode.Should().Be(400);
            model.Value.Should().Be("UserId missing");
        }

        [Test, AutoMoqData]
        public async Task And_CommandMemberIdIsEmptyOrNull_Then_ReturnBadRequest(
            ModifyMemberCommand command,
            ModifyMemberResponse response
        )
        {
            command.UserId = null;

            var result = await ExecuteMediatorCommand(command, response, _memberId);

            result.Should().NotBeNull();
            var model = ((BadRequestObjectResult)result);
            model.StatusCode.Should().Be(400);
            model.Value.Should().Be("UserId missing");
        }

        [Test, AutoMoqData]
        public async Task And_CommandAndRequestMemberIdDoNotMatch_Then_ReturnBadRequest(
            ModifyMemberCommand command,
            ModifyMemberResponse response
        )
        {
            var result = await ExecuteMediatorCommand(command, response, _memberId);

            result.Should().NotBeNull();
            var model = ((BadRequestObjectResult)result);
            model.StatusCode.Should().Be(400);
            model.Value.Should().Be("UserId mismatch");
        }

        [Test, AutoMoqData]
        public async Task And_MediatorFailsToUpdate_Then_ReturnBadRequest(
            ModifyMemberCommand command
        )
        {
            command.UserId = _memberId;

            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).Throws(new Exception());
            var result = await _controller.ManageMembership(command, _memberId);

            result.Should().NotBeNull();
            var model = ((BadRequestObjectResult)result);
            model.StatusCode.Should().Be(400);
            model.Value.Should().Be($"Error attempting to modify member {_memberId}");
        }

        [Test, AutoMoqData]
        public async Task And_MediatorFailsToFindMember_Then_ReturnBadRequest(
            ModifyMemberCommand command
        )
        {
            command.UserId = _memberId;

            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).Throws(new KeyNotFoundException());
            var result = await _controller.ManageMembership(command, _memberId);

            result.Should().NotBeNull();
            var model = ((NotFoundObjectResult)result);
            model.StatusCode.Should().Be(404);
            model.Value.Should().Be($"MemberId is unknown {_memberId}");
        }
        private async Task<IActionResult> ExecuteMediatorCommand(
            ModifyMemberCommand command,
            ModifyMemberResponse response,
            string? userId
        )
        {
            _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            return await _controller.ManageMembership(command, _memberId);
        }
    }
}
