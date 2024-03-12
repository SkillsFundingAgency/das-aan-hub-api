using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Apprentices.Queries.GetApprenticeMember;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.ApprenticesControllerTests;

public class ApprenticesControllerGetTests
{
    [Test, MoqAutoData]
    public async Task GetApprentice_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] ApprenticesController sut,
       Guid apprenticeId)
    {
        await sut.Get(apprenticeId);

        mediatorMock.Verify(m => m.Send(It.Is<GetApprenticeMemberQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task GetApprentice_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        Guid apprenticeId)
    {
        var notFoundResponse = ValidatedResponse<GetMemberResult>.EmptySuccessResponse();
        mediatorMock.Setup(m => m.Send(It.Is<GetApprenticeMemberQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>())).ReturnsAsync(notFoundResponse);

        var result = await sut.Get(apprenticeId);

        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task GetApprentice_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        Guid apprenticeId,
        GetMemberResult getMemberResult)
    {
        var response = new ValidatedResponse<GetMemberResult>(getMemberResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetApprenticeMemberQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.Get(apprenticeId);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getMemberResult);
    }

    [Test, MoqAutoData]
    public async Task GetApprentice_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        List<ValidationFailure> errors,
        Guid apprenticeId)
    {
        var errorResponse = new ValidatedResponse<GetMemberResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetApprenticeMemberQuery>(q => q.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);

        var result = await sut.Get(apprenticeId);

        result.As<BadRequestObjectResult>().Should().NotBeNull();

        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
