using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Members.Queries.GetMemberByEmail;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.MembersControllerTests;

public class MembersControllerGetByEmailTests
{
    [Test, MoqAutoData]
    public async Task GetMemberByEmail_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        string email)
    {
        await sut.Get(email);
        mediatorMock.Verify(m => m.Send(It.Is<GetMemberByEmailQuery>(q => q.Email == email), It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task GetMemberByEmail_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        string email)
    {
        var notFoundResponse = ValidatedResponse<GetMemberByEmailResult>.EmptySuccessResponse();
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberByEmailQuery>(q => q.Email == email), It.IsAny<CancellationToken>())).ReturnsAsync(notFoundResponse);

        var result = await sut.Get(email);
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task GetMemberByEmail_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        string email,
        GetMemberByEmailResult getMemberByEmailResult)
    {
        var response = new ValidatedResponse<GetMemberByEmailResult>(getMemberByEmailResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberByEmailQuery>(q => q.Email == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.Get(email);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getMemberByEmailResult);
    }
}