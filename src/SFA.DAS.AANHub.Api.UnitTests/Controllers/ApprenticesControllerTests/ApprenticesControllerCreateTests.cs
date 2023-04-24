using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.ApprenticesControllerTests;

public class ApprenticesControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task CreateApprentice_InvokesRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        CreateApprenticeMemberCommand command)
    {
        var response = new ValidatedResponse<CreateMemberCommandResponse>(new CreateMemberCommandResponse(command.Id));

        mediatorMock.Setup(m => m.Send(It.IsAny<CreateApprenticeMemberCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.CreateApprentice(command) as CreatedAtActionResult;

        result?.ControllerName.Should().Be("Apprentices");
        result?.ActionName.Should().Be("Get");
        result?.StatusCode.Should().Be(StatusCodes.Status201Created);
        result?.Value.As<CreateMemberCommandResponse>().MemberId.Should().Be(command.Id);
    }

    [Test]
    [MoqAutoData]
    public async Task CreateApprentice_InvokesRequest_BadResultGivesBadRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        CreateApprenticeMemberCommand command)
    {
        var errorResponse = new ValidatedResponse<CreateMemberCommandResponse>
        (new List<ValidationFailure>
        {
            new("Name", "error")
        });

        mediatorMock.Setup(m => m.Send(It.IsAny<CreateApprenticeMemberCommand>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);

        var requestResult = await sut.CreateApprentice(command);

        var result = requestResult as BadRequestObjectResult;
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
}
