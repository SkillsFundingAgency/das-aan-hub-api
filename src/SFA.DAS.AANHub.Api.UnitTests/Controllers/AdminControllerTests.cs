using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Admins.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.UnitTests;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class AdminControllerTests
    {
        private readonly AdminsController _controller;
        private readonly Mock<IMediator> _mediator;

        public AdminControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _controller = new AdminsController(Mock.Of<ILogger<AdminsController>>(), _mediator.Object);
        }

        [Test]
        [AutoMoqData]
        public async Task CreateAdmin_InvokesRequest(
            CreateAdminModel model,
            CreateAdminMemberCommand command)
        {
            var response = new ValidatedResponse<CreateAdminMemberCommandResponse>
            (new CreateAdminMemberCommandResponse
            {
                MemberId = command.Id,
                Status = MembershipStatus.Live
            });

            model.Regions = new List<int>(new[]
            {
                1, 2
            });

            _mediator.Setup(m => m.Send(It.IsAny<CreateAdminMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreateAdmin(Guid.NewGuid(), model);

            result.As<CreatedAtActionResult>().ControllerName.Should().Be("Admins");
            result.As<CreatedAtActionResult>().ActionName.Should().Be("CreateAdmin");
            result.As<CreatedAtActionResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Test]
        [AutoMoqData]
        public async Task CreateAdmin_InvokesRequest_WithErrors(
            CreateAdminMemberCommand command)
        {
            var errorResponse = new ValidatedResponse<CreateAdminMemberCommandResponse>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });


            var model = new CreateAdminModel();
            _mediator.Setup(m => m.Send(It.IsAny<CreateAdminMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);
            var result = await _controller.CreateAdmin(Guid.NewGuid(), model);

            result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}