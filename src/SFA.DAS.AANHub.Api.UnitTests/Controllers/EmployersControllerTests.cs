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
using SFA.DAS.AANHub.Application.Employers.Queries;
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
            var result = await _controller.CreateEmployer(Guid.NewGuid(), model) as CreatedAtActionResult;

            result?.ControllerName.Should().Be("Employers");
            result?.ActionName.Should().Be("CreateEmployer");
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.As<CreateEmployerMemberCommandResponse>().MemberId.Should().Be(command.Id);
        }

        [Test]
        [AutoMoqData]
        public async Task CreateEmployer_InvokesRequest_WithErrors()
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

        [Test]
        [AutoMoqData]
        public async Task GetEmployer_InvokesQueryHandler(
            ValidatedResponse<GetEmployerMemberResult> handlerResult)
        {
            _mediator.Setup(m => m.Send(It.IsAny<GetEmployerMemberQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(handlerResult);

            var userRef = new Guid();
            var response = await _controller.GetEmployer(userRef);
            response.Should().NotBeNull();

            var result = response as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);

            var queryResult = result.Value as GetEmployerMemberResult;
            queryResult.Should().BeEquivalentTo(handlerResult.Result);

            _mediator.Verify(m => m.Send(It.IsAny<GetEmployerMemberQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetEmployer_InvokesQueryHandler_NoResultGivesNotFound()
        {
            var errorResponse = new ValidatedResponse<GetEmployerMemberResult>
                (new List<ValidationFailure>());

            var userRef = Guid.NewGuid();
            _mediator.Setup(m => m.Send(It.Is<GetEmployerMemberQuery>(q => q.UserRef == userRef),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResponse);

            var response = await _controller.GetEmployer(userRef);

            var result = response as NotFoundResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status404NotFound, result!.StatusCode);

            _mediator.Verify(m => m.Send(It.IsAny<GetEmployerMemberQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetEmployer_InvokesQueryHandler_ResultGivesSuccessfulResult()
        {
            var response = new ValidatedResponse<GetEmployerMemberResult>
            (new GetEmployerMemberResult
            {
                Email = "email@email.com",
                MemberId = new Guid(),
                Name = "name"
            });

            var userRef = Guid.NewGuid();
            _mediator.Setup(m => m.Send(It.Is<GetEmployerMemberQuery>(q => q.UserRef == userRef),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var getResult = await _controller.GetEmployer(userRef);

            var result = getResult as OkObjectResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        }

        [Test]
        [AutoMoqData]
        public async Task GetEmployer_InvokesQueryHandler_BadResultGivesBadRequest()
        {
            var errorResponse = new ValidatedResponse<GetEmployerMemberResult>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });

            var userRef = Guid.NewGuid();
            _mediator.Setup(m => m.Send(It.Is<GetEmployerMemberQuery>(q => q.UserRef == userRef),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResponse);

            var response = await _controller.GetEmployer(userRef);

            var result = response as BadRequestObjectResult;
            result.Should().NotBeNull();

            var errorList = result?.Value as List<ValidationFailure>;
            errorList?.Count.Should().Be(1);
            errorList?[0].Should().Be(new ValidationFailure
            {
                PropertyName = "name",
                ErrorMessage = "error"
            });

            Assert.AreEqual(StatusCodes.Status400BadRequest, result!.StatusCode);
        }
    }
}