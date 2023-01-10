using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Employers.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.UnitTests;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class EmployersControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly EmployersController _controller;
        public EmployersControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _controller = new EmployersController(Mock.Of<ILogger<EmployersController>>(), _mediator.Object);

        }
        [Test, AutoMoqData]
        public async Task CreateEmployer_InvokesRequest(
            CreateEmployerModel model,
            CreateEmployerMemberCommand command)
        {
            var response = new ValidatableResponse<CreateEmployerMemberCommandResponse>
            (new CreateEmployerMemberCommandResponse
            {
                MemberId = command.Id,
                Status = Constants.MembershipStatus.Live,
            });

            model.Regions = new List<int>(new[] { 1, 2, });
            _mediator.Setup(m => m.Send(It.IsAny<CreateEmployerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreateEmployer(Guid.NewGuid(), model);

            result.As<CreatedAtActionResult>().ControllerName.Should().Be("Employers");
            result.As<CreatedAtActionResult>().ActionName.Should().Be("CreateEmployer");
            result.As<CreatedAtActionResult>().StatusCode.Should().Be(201);
        }

        [Test, AutoMoqData]
        public async Task CreateEmployer_InvokesRequest_WithErrors(
            CreateEmployerMemberCommand command)
        {
            var response = new ValidatableResponse<CreateEmployerMemberCommandResponse>
            (new CreateEmployerMemberCommandResponse
            {
                MemberId = command.Id,
                Status = Constants.MembershipStatus.Live.ToString()
            }, new List<string> { new string("Error") });
            var model = new CreateEmployerModel();
            _mediator.Setup(m => m.Send(It.IsAny<CreateEmployerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreateEmployer(Guid.NewGuid(), model);

            result.As<BadRequestObjectResult>().StatusCode.Should().Be(400);
        }
    }
}
