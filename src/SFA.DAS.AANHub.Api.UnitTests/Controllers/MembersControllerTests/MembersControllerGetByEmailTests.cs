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
        var notFoundResponse = ValidatedResponse<GetMemberResult>.EmptySuccessResponse();
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
        GetMemberResult getMemberByEmailResult)
    {
        var response = new ValidatedResponse<GetMemberResult>(getMemberByEmailResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberByEmailQuery>(q => q.Email == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.Get(email);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getMemberByEmailResult);
    }

    [Test]
    [MoqAutoData]
    public async Task GetAdmin_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        List<ValidationFailure> errors,
        string email)
    {
        var errorResponse = new ValidatedResponse<GetMemberResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberByEmailQuery>(q => q.Email == email), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);

        var result = await sut.Get(email);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}