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
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberRemove;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.MembersControllerTests;
public class MembersControllerPostTests
{
    [Test]
    [MoqAutoData]
    public async Task PostMemberStatus_InvokesRequest_ReturnsNoContentResult(
    [Frozen] Mock<IMediator> mediatorMock,
    [Greedy] MembersController sut,
    Guid requestedByMemberId,
    Guid memberId,
    MembershipStatusType status,
    CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(true));

        var request = new PostMemberStatusModel { Status = status };

        mediatorMock.Setup(m => m.Send(It.Is<PostMemberRemoveCommand>(c => c.MemberId == memberId
                                                                           && c.AdminMemberId == requestedByMemberId
                                                                           && c.Status == status),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PostMemberStatus(requestedByMemberId, memberId, request, cancellationToken);

        (result as NoContentResult).Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task PostMemberStatus_ResponseUnsuccessful_ReturnsNotFoundResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid requestedByMemberId,
        Guid memberId,
        MembershipStatusType status,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult(false));

        var request = new PostMemberStatusModel { Status = status };

        mediatorMock.Setup(m => m.Send(It.Is<PostMemberRemoveCommand>(c => c.MemberId == memberId
                                                                           && c.AdminMemberId == requestedByMemberId
                                                                           && c.Status == status),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PostMemberStatus(requestedByMemberId, memberId, request, cancellationToken);

        result.Should().NotBeNull();
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task PostMemberStatus_HasValidationErrors_ReturnsBadRequestResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid requestedByMemberId,
        Guid memberId,
        MembershipStatusType status,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<SuccessCommandResult>(new List<ValidationFailure> { new("Name", "error") });
        var request = new PostMemberStatusModel { Status = status };
        mediatorMock.Setup(m => m.Send(It.Is<PostMemberRemoveCommand>(c => c.MemberId == memberId
                                                                           && c.AdminMemberId == requestedByMemberId
                                                                           && c.Status == status),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PostMemberStatus(requestedByMemberId, memberId, request, cancellationToken);
        result.As<BadRequestObjectResult>().Should().NotBeNull();
    }
}
