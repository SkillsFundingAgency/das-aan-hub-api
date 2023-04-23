﻿using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.AdminControllerTests;

public class AdminsControllerCreateTests
{
    [Test]
    [MoqAutoData]
    public async Task CreateAdmin_InvokesRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        CreateAdminModel model,
        CreateAdminMemberCommand command)
    {
        var response = new ValidatedResponse<CreateMemberCommandResponse>(new CreateMemberCommandResponse(command.Id));
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateAdminMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.CreateAdmin(model) as CreatedAtActionResult;

        result?.ControllerName.Should().Be("Admins");
        result?.ActionName.Should().Be("CreateAdmin");
        result?.StatusCode.Should().Be(StatusCodes.Status201Created);
        result?.Value.As<CreateMemberCommandResponse>().MemberId.Should().Be(command.Id);
    }

    [Test]
    [MoqAutoData]
    public async Task CreateAdmin_InvokesRequest_WithErrors(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AdminsController sut,
        List<ValidationFailure> errors)
    {
        var errorResponse = new ValidatedResponse<CreateMemberCommandResponse>(errors);
        var model = new CreateAdminModel();
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateAdminMemberCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);

        var result = await sut.CreateAdmin(model);

        result.As<BadRequestObjectResult>().StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}
