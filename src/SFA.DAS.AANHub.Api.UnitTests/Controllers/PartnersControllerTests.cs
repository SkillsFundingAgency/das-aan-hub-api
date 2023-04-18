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
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;
using SFA.DAS.AANHub.Application.Partners.Commands.PatchPartnerMember;
using SFA.DAS.AANHub.Application.Partners.Queries;
using SFA.DAS.AANHub.Application.UnitTests;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class PartnersControllerTests
    {
        private readonly PartnersController _controller;
        private readonly Mock<IMediator> _mediator;

        public PartnersControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _controller = new PartnersController(Mock.Of<ILogger<PartnersController>>(), _mediator.Object);
        }

        [Test]
        [AutoMoqData]
        public async Task CreatePartners_InvokesRequest(
            CreatePartnerModel model,
            CreatePartnerMemberCommand command)
        {
            var response = new ValidatedResponse<CreateMemberCommandResponse>
            (new CreateMemberCommandResponse(command.Id));

            _mediator.Setup(m => m.Send(It.IsAny<CreatePartnerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await _controller.CreatePartner(model) as CreatedAtActionResult;

            result?.ControllerName.Should().Be("Partners");
            result?.ActionName.Should().Be("CreatePartner");
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
            result?.Value.As<CreateMemberCommandResponse>().MemberId.Should().Be(command.Id);
        }

        [Test]
        [AutoMoqData]
        public async Task CreatePartner_InvokesRequest_WithErrors()
        {
            var errorResponse = new ValidatedResponse<CreateMemberCommandResponse>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });


            var model = new CreatePartnerModel();
            _mediator.Setup(m => m.Send(It.IsAny<CreatePartnerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);
            var result = await _controller.CreatePartner(model);

            result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Test, AutoMoqData]
        public async Task GetPartner_InvokesQueryHandler_Returns200Response()
        {
            var response = new ValidatedResponse<GetPartnerMemberResult>
            (new GetPartnerMemberResult
            {
                Email = "",
                Name = "",
                Organisation = "",
                MemberId = Guid.NewGuid()
            });

            string userName = "username";
            _mediator.Setup(m => m.Send(It.IsAny<GetPartnerMemberQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _controller.GetPartner(userName);
            result.Should().NotBeNull();

            result.As<OkObjectResult>().StatusCode.Should().NotBeNull();
        }

        [Test, AutoMoqData]
        public async Task GetPartner_QueryHandlerReturnsValudationErrors_ReturnsBadRequestResponse()
        {
            var response = new ValidatedResponse<GetPartnerMemberResult>(new List<ValidationFailure> { new("Name", "error") });

            string userName = "username";
            _mediator.Setup(m => m.Send(It.IsAny<GetPartnerMemberQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _controller.GetPartner(userName);

            result.As<BadRequestObjectResult>().StatusCode.Should().NotBeNull();
        }

        [Test]
        [AutoMoqData]
        public async Task GetPartner_InvokesQueryHandler(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] PartnersController sut,
            string userName,
            ValidatedResponse<GetPartnerMemberResult> handlerResult)
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<GetPartnerMemberQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(handlerResult);

            var response = await sut.GetPartner(userName);
            response.Should().NotBeNull();

            var result = response as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);

            var queryResult = result.Value as GetPartnerMemberResult;
            queryResult.Should().BeEquivalentTo(handlerResult.Result);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetPartnerMemberQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetPartner_QueryHandlerResponseHasNoResult_ReturnsNotFoundResponse(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] PartnersController sut)
        {
            string userName = "username";
            var errorResponse = new ValidatedResponse<GetPartnerMemberResult>(new List<ValidationFailure>());

            mediatorMock
                .Setup(m => m.Send(It.Is<GetPartnerMemberQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResponse);

            var response = await sut.GetPartner(userName);

            var result = response as NotFoundResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status404NotFound, result!.StatusCode);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetPartnerMemberQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetPartner_InvokesQueryHandler_ResultGivesSuccessfulResult(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] PartnersController sut)
        {
            string userName = "username";
            var response = new ValidatedResponse<GetPartnerMemberResult>
            (new GetPartnerMemberResult
            {
                Email = "email@email.com",
                MemberId = Guid.NewGuid(),
                Name = "name",
                Organisation = "organisation"
            });

            mediatorMock.Setup(m => m.Send(It.Is<GetPartnerMemberQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var getResult = await sut.GetPartner(userName);

            var result = getResult as OkObjectResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        }

        [Test]
        [AutoMoqData]
        public async Task GetPartner_InvokesQueryHandler_BadResultGivesBadRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] PartnersController sut)
        {
            string userName = "username";
            var errorResponse = new ValidatedResponse<GetPartnerMemberResult>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });

            mediatorMock.Setup(m => m.Send(It.Is<GetPartnerMemberQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResponse);

            var response = await sut.GetPartner(userName);

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
        public async Task PatchPartner_InvokesRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] PartnersController sut,
            Guid requestedByMemberId, string userName)
        {
            var Email = "Email";
            var testValue = "value";

            var patchDoc = new JsonPatchDocument<Partner>();
            patchDoc.Operations.Add(new Operation<Partner>
            {
                op = nameof(OperationType.Replace),
                path = Email,
                value = testValue
            });

            var success = true;
            var response = new ValidatedResponse<PatchMemberCommandResponse>
                (new PatchMemberCommandResponse(success));

            mediatorMock.Setup(m => m.Send(It.Is<PatchPartnerMemberCommand>(c => c.RequestedByMemberId == requestedByMemberId && c.UserName == userName),
                It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await sut.PatchPartner(requestedByMemberId, userName, patchDoc);

            (result as NoContentResult).Should().NotBeNull();
        }

        [Test]
        [AutoMoqData]
        public async Task PatchPartner_InvokesRequest_NotFound(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] PartnersController sut,
            Guid requestedByMemberId, string userName)
        {
            var Email = "Email";
            var testValue = "value";

            var patchDoc = new JsonPatchDocument<Partner>();
            patchDoc.Operations.Add(new Operation<Partner>
            {
                op = nameof(OperationType.Replace),
                path = Email,
                value = testValue
            });

            var response = new ValidatedResponse<PatchMemberCommandResponse>(new PatchMemberCommandResponse(false));
            mediatorMock.Setup(m => m.Send(It.IsAny<PatchPartnerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await sut.PatchPartner(requestedByMemberId, userName, patchDoc);

            result.Should().NotBeNull();
            result.As<NotFoundResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        [AutoMoqData]
        public async Task PatchPartner_InvokesRequest_WithErrors(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] PartnersController sut,
            Guid requestedByMemberId, string userName)
        {
            var Email = "Email";
            var testValue = "value";

            var patchDoc = new JsonPatchDocument<Partner>();
            patchDoc.Operations.Add(new Operation<Partner>
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

            mediatorMock.Setup(m => m.Send(It.IsAny<PatchPartnerMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await sut.PatchPartner(requestedByMemberId, userName, patchDoc);

            result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}