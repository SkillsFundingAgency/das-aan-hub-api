using AutoFixture.NUnit3;
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
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;
using SFA.DAS.AANHub.Application.Admins.Queries;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.UnitTests;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class AdminsControllerTests
    {
        private readonly AdminsController _controller;
        private readonly Mock<IMediator> _mediator;

        public AdminsControllerTests()
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
            var response = new ValidatedResponse<CreateMemberCommandResponse>(new CreateMemberCommandResponse(command.Id));

            _mediator.Setup(m => m.Send(It.IsAny<CreateAdminMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreateAdmin(model) as CreatedAtActionResult;

            result?.ControllerName.Should().Be("Admins");
            result?.ActionName.Should().Be("CreateAdmin");
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.As<CreateMemberCommandResponse>().MemberId.Should().Be(command.Id);
        }

        [Test]
        public async Task CreateAdmin_InvokesRequest_WithErrors()
        {
            var errorResponse = new ValidatedResponse<CreateMemberCommandResponse>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });


            var model = new CreateAdminModel();
            _mediator.Setup(m => m.Send(It.IsAny<CreateAdminMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);
            var result = await _controller.CreateAdmin(model);

            result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Test]
        [AutoMoqData]
        public async Task GetAdmin_InvokesQueryHandler(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] AdminsController sut,
            string userName,
            ValidatedResponse<GetAdminMemberResult> handlerResult)
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAdminMemberQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(handlerResult);

            var response = await sut.GetAdmin(userName);
            response.Should().NotBeNull();

            var result = response as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);

            var queryResult = result.Value as GetAdminMemberResult;
            queryResult.Should().BeEquivalentTo(handlerResult.Result);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetAdminMemberQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetAdmin_InvokesQueryHandler_NoResultGivesNotFound(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] AdminsController sut,
            string userName)
        {
            var errorResponse = new ValidatedResponse<GetAdminMemberResult>
                (new List<ValidationFailure>());

            mediatorMock.Setup(m => m.Send(It.Is<GetAdminMemberQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResponse);

            var response = await sut.GetAdmin(userName);

            var result = response as NotFoundResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status404NotFound, result!.StatusCode);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetAdminMemberQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetAdmin_InvokesQueryHandler_ResultGivesSuccessfulResult(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] AdminsController sut,
            string userName)
        {
            var response = new ValidatedResponse<GetAdminMemberResult>
            (new GetAdminMemberResult
            {
                Email = "email@email.com",
                MemberId = Guid.NewGuid(),
                Name = "name"
            });

            mediatorMock.Setup(m => m.Send(It.Is<GetAdminMemberQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var getResult = await sut.GetAdmin(userName);

            var result = getResult as OkObjectResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        }

        [Test]
        [AutoMoqData]
        public async Task GetAdmin_InvokesQueryHandler_BadResultGivesBadRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] AdminsController sut, string userName)
        {
            var errorResponse = new ValidatedResponse<GetAdminMemberResult>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });

            mediatorMock.Setup(m => m.Send(It.Is<GetAdminMemberQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResponse);

            var response = await sut.GetAdmin(userName);

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
