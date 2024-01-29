using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberReinstate;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.MembersControllerTests;
public class MembersControllerPostReinstateTests
{
    [Test, MoqAutoData]
    public async Task PostMemberReinstate_InvokesRequest_ReturnsNoContentResult(
             [Frozen] Mock<IMediator> mediatorMock,
             [Greedy] MembersController sut,
             Guid memberId,
             CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(true));
        mediatorMock.Setup(m => m.Send(It.Is<PostMemberReinstateCommand>(c => c.MemberId == memberId),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PostMemberReinstate(memberId, cancellationToken);

        (result as NoContentResult).Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task PostMemberReinstate_ResponseUnsuccessful_ReturnsNotFoundResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid memberId,
        List<int> leavingReasons,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(false));

        mediatorMock.Setup(m => m.Send(It.Is<PostMemberReinstateCommand>(c =>
                c.MemberId == memberId),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PostMemberReinstate(memberId, cancellationToken);

        result.Should().NotBeNull();
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task PostMemberReinstate_HasValidationErrors_ReturnsBadRequestResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid memberId,
        List<int> leavingReasons,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new List<ValidationFailure> { new("Name", "error") });

        mediatorMock.Setup(m => m.Send(It.Is<PostMemberReinstateCommand>(
                c => c.MemberId == memberId),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PostMemberReinstate(memberId, cancellationToken);
        result.As<BadRequestObjectResult>().Should().NotBeNull();
    }
}
