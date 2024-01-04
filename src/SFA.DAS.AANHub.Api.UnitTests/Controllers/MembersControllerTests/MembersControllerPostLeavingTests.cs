using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberLeaving;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.MembersControllerTests;
public class MembersControllerPostLeavingTests
{
    [Test]
    [MoqAutoData]
    public async Task PostMemberLeaving_InvokesRequest_ReturnsNoContentResult(
    [Frozen] Mock<IMediator> mediatorMock,
    [Greedy] MembersController sut,
    Guid memberId,
    List<int> leavingReasons,
    CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(true));

        var request = new PostMemberLeavingModel { LeavingReasons = leavingReasons };

        mediatorMock.Setup(m => m.Send(It.Is<PostMemberLeavingCommand>(c => c.MemberId == memberId
                                                                           && c.LeavingReasons == leavingReasons),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PostMemberLeaving(memberId, request, cancellationToken);

        (result as NoContentResult).Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task PostMemberLeaving_ResponseUnsuccessful_ReturnsNotFoundResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid memberId,
        List<int> leavingReasons,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(false));

        var request = new PostMemberLeavingModel { LeavingReasons = leavingReasons };

        mediatorMock.Setup(m => m.Send(It.Is<PostMemberLeavingCommand>(c =>
                c.MemberId == memberId && c.LeavingReasons == leavingReasons),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PostMemberLeaving(memberId, request, cancellationToken);

        result.Should().NotBeNull();
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task PostMemberLeaving_HasValidationErrors_ReturnsBadRequestResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid memberId,
        List<int> leavingReasons,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new List<ValidationFailure> { new("Name", "error") });
        var request = new PostMemberLeavingModel { LeavingReasons = leavingReasons };
        mediatorMock.Setup(m => m.Send(It.Is<PostMemberLeavingCommand>(
                c => c.MemberId == memberId && c.LeavingReasons == leavingReasons),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PostMemberLeaving(memberId, request, cancellationToken);
        result.As<BadRequestObjectResult>().Should().NotBeNull();
    }
}
