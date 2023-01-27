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
using SFA.DAS.AANHub.Application.Employers.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.UnitTests;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class EmployersControllerTests
    {
        private readonly EmployersController _controller;
        private readonly Mock<IMediator> _mediator;

        public EmployersControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _controller = new EmployersController(Mock.Of<ILogger<EmployersController>>(), _mediator.Object);
        }

        [Test]
        [AutoMoqData]
        public async Task CreateEmployer_InvokesRequest(
            CreateEmployerModel model,
            CreateEmployerMemberCommand command)
        {
            var response = new ValidatedResponse<CreateEmployerMemberCommandResponse>
            (new CreateEmployerMemberCommandResponse
            {
                MemberId = command.Id,
                Status = MembershipStatus.Live
            });

            model.Regions = new List<int>(new[]
            {
                1, 2
            });

            _mediator.Setup(m => m.Send(It.IsAny<CreateEmployerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreateEmployer(Guid.NewGuid(), model);

            result.As<CreatedAtActionResult>().ControllerName.Should().Be("Employers");
            result.As<CreatedAtActionResult>().ActionName.Should().Be("CreateEmployer");
            result.As<CreatedAtActionResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Test]
        [AutoMoqData]
        public async Task CreateEmployer_InvokesRequest_WithErrors(
            CreateEmployerMemberCommand command)
        {
            var errorResponse = new ValidatedResponse<CreateEmployerMemberCommandResponse>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });


            var model = new CreateEmployerModel();
            _mediator.Setup(m => m.Send(It.IsAny<CreateEmployerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);
            var result = await _controller.CreateEmployer(Guid.NewGuid(), model);

            result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}