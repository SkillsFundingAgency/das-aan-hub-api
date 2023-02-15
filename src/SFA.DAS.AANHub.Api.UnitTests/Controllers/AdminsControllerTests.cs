using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;
using SFA.DAS.AANHub.Application.Admins.Commands.PatchAdminMember;
using SFA.DAS.AANHub.Application.Admins.Queries;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.UnitTests;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

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
            var result = await _controller.CreateAdmin(Guid.NewGuid(), model) as CreatedAtActionResult;

            result?.ControllerName.Should().Be("Admins");
            result?.ActionName.Should().Be("CreateAdmin");
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.As<CreateAdminMemberCommandResponse>().MemberId.Should().Be(command.Id);
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
                MemberId = new Guid(),
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

        [Test]
        [AutoMoqData]
        public async Task PatchAdmin_InvokesRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] AdminsController sut,
            Guid userId, string userName)
        {
            var email = "Email";
            var testValue = "value";

            var patchDoc = new JsonPatchDocument<Admin>();
            patchDoc.Operations.Add(new Operation<Admin>
            {
                op = nameof(OperationType.Replace),
                path = email,
                value = testValue
            });

            var success = true;
            var response = new ValidatedResponse<PatchMemberCommandResponse>
                (new PatchMemberCommandResponse(success));

            mediatorMock.Setup(m => m.Send(It.Is<PatchAdminMemberCommand>(c => c.RequestedByMemberId == userId && c.UserName == userName),
                It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await sut.PatchAdmin(userId, userName, patchDoc);

            (result as NoContentResult).Should().NotBeNull();
        }

        [Test]
        [AutoMoqData]
        public async Task PatchAdmin_InvokesRequest_NotFound(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] AdminsController sut,
            Guid userId, string userName)
        {
            var email = "Email";
            var testValue = "value";

            var patchDoc = new JsonPatchDocument<Admin>();
            patchDoc.Operations.Add(new Operation<Admin>
            {
                op = nameof(OperationType.Replace),
                path = email,
                value = testValue
            });

            var response = new ValidatedResponse<PatchMemberCommandResponse>(new PatchMemberCommandResponse(false));
            mediatorMock.Setup(m => m.Send(It.IsAny<PatchAdminMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await sut.PatchAdmin(userId, userName, patchDoc);

            result.Should().NotBeNull();
            result.As<NotFoundResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        [AutoMoqData]
        public async Task PatchAdmin_InvokesRequest_WithErrors(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] AdminsController sut,
            Guid userId, string userName)
        {
            var email = "Email";
            var testValue = "value";

            var patchDoc = new JsonPatchDocument<Admin>();
            patchDoc.Operations.Add(new Operation<Admin>
            {
                op = nameof(OperationType.Replace),
                path = email,
                value = testValue
            });

            var response = new ValidatedResponse<PatchMemberCommandResponse>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });

            mediatorMock.Setup(m => m.Send(It.IsAny<PatchAdminMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await sut.PatchAdmin(userId, userName, patchDoc);

            result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}