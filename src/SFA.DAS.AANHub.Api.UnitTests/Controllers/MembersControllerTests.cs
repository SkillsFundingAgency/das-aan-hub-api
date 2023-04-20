﻿using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Application.UnitTests;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;

public class MembersControllerTests
{
    [Test]
    [AutoMoqData]
    public async Task PatchMember_InvokesRequest_ReturnsNoContentResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid memberId,
        JsonPatchDocument<Member> patchDoc,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<CommandResult>(new CommandResult(true));

        mediatorMock.Setup(m => m.Send(It.Is<PatchMemberCommand>(c => c.MemberId == memberId),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PatchMember(memberId, patchDoc, cancellationToken);

        (result as NoContentResult).Should().NotBeNull();
    }

    [Test]
    [AutoMoqData]
    public async Task PatchMember_ResponseUnsuccessfull_ReturnsNotFoundResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid memberId,
        JsonPatchDocument<Member> patchDoc,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<CommandResult>(new CommandResult(false));
        mediatorMock.Setup(m => m.Send(It.Is<PatchMemberCommand>(c => c.MemberId == memberId),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PatchMember(memberId, patchDoc, cancellationToken);

        result.Should().NotBeNull();
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [AutoMoqData]
    public async Task PatchMember_HasValidationErrors_ReturnsBadRequestResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid memberId,
        JsonPatchDocument<Member> patchDoc,
        CancellationToken cancellationToken)
    {
        var response = new ValidatedResponse<CommandResult>(new List<ValidationFailure> { new("Name", "error") });
        mediatorMock.Setup(m => m.Send(It.IsAny<PatchMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.PatchMember(memberId, patchDoc, cancellationToken);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
    }
}
