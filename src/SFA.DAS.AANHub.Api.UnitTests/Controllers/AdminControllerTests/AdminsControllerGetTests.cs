﻿using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Admins.Queries;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.AdminControllerTests;

public class AdminsControllerGetTests
{
    [Test]
    [MoqAutoData]
    public async Task GetAdmin_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        string userName)
    {
        await sut.GetAdmin(userName);

        mediatorMock.Verify(m => m.Send(It.Is<GetAdminMemberQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task GetAdmin_InvokesQueryHandler_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        string userName)
    {
        var notFoundResponse = ValidatedResponse<GetMemberResult>.EmptySuccessResponse();
        mediatorMock.Setup(m => m.Send(It.Is<GetAdminMemberQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()))
            .ReturnsAsync(notFoundResponse);

        var result = await sut.GetAdmin(userName);

        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task GetAdmin_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        string userName,
        GetMemberResult getMemberResult)
    {
        var response = new ValidatedResponse<GetMemberResult>(getMemberResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetAdminMemberQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetAdmin(userName);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getMemberResult);
    }

    [Test]
    [MoqAutoData]
    public async Task GetAdmin_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        List<ValidationFailure> errors,
        string userName)
    {
        var errorResponse = new ValidatedResponse<GetMemberResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetAdminMemberQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);

        var result = await sut.GetAdmin(userName);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
