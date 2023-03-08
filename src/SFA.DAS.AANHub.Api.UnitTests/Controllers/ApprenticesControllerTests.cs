using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember;
using SFA.DAS.AANHub.Application.Apprentices.Queries;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.UnitTests;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class ApprenticesControllerTests
    {
        [Test]
        [AutoMoqData]
        public async Task CreateApprentice_InvokesRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticesController sut,
            CreateApprenticeModel model, CreateApprenticeMemberCommand command)
        {
            var response = new ValidatedResponse<CreateApprenticeMemberCommandResponse>(new CreateApprenticeMemberCommandResponse
            {
                MemberId = command.Id,
                Status = MembershipStatus.Live
            });


            mediatorMock.Setup(m => m.Send(It.IsAny<CreateApprenticeMemberCommand>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await sut.CreateApprentice(Guid.NewGuid(), model) as CreatedAtActionResult;

            result?.ControllerName.Should().Be("Apprentices");
            result?.ActionName.Should().Be("GetApprentice");
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.As<CreateApprenticeMemberCommandResponse>().MemberId.Should().Be(command.Id);
        }

        [Test]
        [AutoMoqData]
        public async Task CreateApprentice_InvokesRequest_BadResultGivesBadRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticesController sut,
            CreateApprenticeModel model)
        {
            var errorResponse = new ValidatedResponse<CreateApprenticeMemberCommandResponse>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });

            mediatorMock.Setup(m => m.Send(It.IsAny<CreateApprenticeMemberCommand>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);

            var requestResult = await sut.CreateApprentice(Guid.NewGuid(), model);

            var result = requestResult as BadRequestObjectResult;
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
        public async Task GetApprentice_InvokesQueryHandler(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticesController sut,
            Guid apprenticeId,
            ValidatedResponse<GetApprenticeMemberResult> handlerResult)
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<GetApprenticeMemberQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(handlerResult);

            var response = await sut.GetApprentice(apprenticeId);
            response.Should().NotBeNull();

            var result = response as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);

            var queryResult = result.Value as GetApprenticeMemberResult;
            queryResult.Should().BeEquivalentTo(handlerResult.Result);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetApprenticeMemberQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetApprentice_InvokesQueryHandler_NoResultGivesNotFound(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticesController sut,
            Guid apprenticeId)
        {
            var errorResponse = new ValidatedResponse<GetApprenticeMemberResult>
                (new List<ValidationFailure>());

            mediatorMock.Setup(m => m.Send(It.Is<GetApprenticeMemberQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResponse);

            var response = await sut.GetApprentice(apprenticeId);

            var result = response as NotFoundResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status404NotFound, result!.StatusCode);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetApprenticeMemberQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetApprentice_InvokesQueryHandler_ResultGivesSuccessfulResult(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticesController sut,
            Guid apprenticeId,
            GetApprenticeMemberResult getApprenticeMemberResult)
        {
            var response = new ValidatedResponse<GetApprenticeMemberResult>(getApprenticeMemberResult);

            mediatorMock.Setup(m => m.Send(It.Is<GetApprenticeMemberQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var getResult = await sut.GetApprentice(apprenticeId);

            var result = getResult as OkObjectResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        }

        [Test]
        [AutoMoqData]
        public async Task GetApprentice_InvokesQueryHandler_BadResultGivesBadRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticesController sut,
            Guid apprenticeId)
        {
            var errorResponse = new ValidatedResponse<GetApprenticeMemberResult>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });

            mediatorMock.Setup(m => m.Send(It.Is<GetApprenticeMemberQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResponse);

            var response = await sut.GetApprentice(apprenticeId);

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
        public async Task PatchApprentice_InvokesRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticesController sut,
            Guid userId,
            Guid apprenticeId)
        {
            var Email = "Email";
            var testValue = "value";

            var patchDoc = new JsonPatchDocument<Apprentice>();
            patchDoc.Operations.Add(new Operation<Apprentice>
            {
                op = nameof(OperationType.Replace),
                path = Email,
                value = testValue
            });

            var success = true;
            var response = new ValidatedResponse<PatchMemberCommandResponse>
                (new PatchMemberCommandResponse(success));

            mediatorMock.Setup(m => m.Send(It.Is<PatchApprenticeMemberCommand>(c => c.RequestedByMemberId == userId && c.ApprenticeId == apprenticeId),
                It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await sut.PatchApprentice(userId, apprenticeId, patchDoc);

            (result as NoContentResult).Should().NotBeNull();
        }

        [Test]
        [AutoMoqData]
        public async Task PatchApprentice_InvokesRequest_NotFound(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticesController sut,
            Guid userId,
            Guid apprenticeId)
        {
            var Email = "Email";
            var testValue = "value";

            var patchDoc = new JsonPatchDocument<Apprentice>();
            patchDoc.Operations.Add(new Operation<Apprentice>
            {
                op = nameof(OperationType.Replace),
                path = Email,
                value = testValue
            });

            var response = new ValidatedResponse<PatchMemberCommandResponse>(new PatchMemberCommandResponse(false));
            mediatorMock.Setup(m => m.Send(It.IsAny<PatchApprenticeMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await sut.PatchApprentice(userId, apprenticeId, patchDoc);

            result.Should().NotBeNull();
            result.As<NotFoundResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        [AutoMoqData]
        public async Task PatchApprentice_InvokesRequest_WithErrors(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticesController sut,
            Guid userId,
            Guid apprenticeId)
        {
            var Email = "Email";
            var testValue = "value";

            var patchDoc = new JsonPatchDocument<Apprentice>();
            patchDoc.Operations.Add(new Operation<Apprentice>
            {
                op = nameof(OperationType.Replace),
                path = Email,
                value = testValue
            });

            var response = new ValidatedResponse<PatchMemberCommandResponse>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });

            mediatorMock.Setup(m => m.Send(It.IsAny<PatchApprenticeMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await sut.PatchApprentice(userId, apprenticeId, patchDoc);

            result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}