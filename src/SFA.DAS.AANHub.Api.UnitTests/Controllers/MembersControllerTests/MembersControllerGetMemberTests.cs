using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Members.Queries.GetMember;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.MembersControllerTests;

public class MembersControllerGetMemberTests
{
    [Test, MoqAutoData]
    public async Task GetMember_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid memberId)
    {
        await sut.GetMember(memberId);
        mediatorMock.Verify(m => m.Send(It.Is<GetMemberQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task GetMember_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid memberId)
    {
        var notFoundResponse = ValidatedResponse<GetMemberResult>.EmptySuccessResponse();
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>())).ReturnsAsync(notFoundResponse);

        var result = await sut.GetMember(memberId);
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task GetMember_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid memberId,
        GetMemberResult getMemberResult)
    {
        var response = new ValidatedResponse<GetMemberResult>(getMemberResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetMember(memberId);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getMemberResult);
    }

    [Test]
    [MoqAutoData]
    public async Task GetMember_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        List<ValidationFailure> errors,
        Guid memberId)
    {
        var errorResponse = new ValidatedResponse<GetMemberResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);

        var result = await sut.GetMember(memberId);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
