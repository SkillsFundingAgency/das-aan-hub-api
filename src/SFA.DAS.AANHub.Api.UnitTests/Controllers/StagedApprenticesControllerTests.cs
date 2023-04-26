using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.StagedApprentices.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;

public class StagedApprenticesControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task GetApprentice_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] StagedApprenticesController sut,
        string lastname,
        DateTime dateofbirth,
        string email,
        GetStagedApprenticeQueryResult expectedQueryResult)
    {
        ValidatedResponse<GetStagedApprenticeQueryResult> expectedResult = new(expectedQueryResult);
        mediatorMock.Setup(m => m.Send(It.IsAny<GetStagedApprenticeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        var actualResult = await sut.GetStagedApprentice(lastname, dateofbirth, email);

        actualResult.As<OkObjectResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task GetApprentice_HandlerReturnsNoData_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] StagedApprenticesController sut,
        string lastname,
        DateTime dateofbirth,
        string email)
    {
        var errorResponse = ValidatedResponse<GetStagedApprenticeQueryResult>.EmptySuccessResponse();

        mediatorMock
            .Setup(m => m.Send(It.Is<GetStagedApprenticeQuery>(q =>
                q.LastName == lastname
                && q.DateOfBirth == dateofbirth
                && q.Email == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorResponse);

        var result = await sut.GetStagedApprentice(lastname, dateofbirth, email);

        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task GetApprentice_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] StagedApprenticesController sut,
        string lastname,
        DateTime dateofbirth,
        string email,
        GetStagedApprenticeQueryResult getStagedApprenticeResult)
    {
        var response = new ValidatedResponse<GetStagedApprenticeQueryResult>(getStagedApprenticeResult);

        mediatorMock
            .Setup(m => m.Send(It.Is<GetStagedApprenticeQuery>(q =>
                q.LastName == lastname
                && q.DateOfBirth == dateofbirth
                && q.Email == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetStagedApprentice(lastname, dateofbirth, email);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getStagedApprenticeResult);
    }

    [Test]
    [MoqAutoData]
    public async Task GetApprentice_InvokesQueryHandler_BadResultGivesBadRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] StagedApprenticesController sut,
        string lastname,
        DateTime dateofbirth,
        string email,
        List<ValidationFailure> errors)
    {
        var errorResponse = new ValidatedResponse<GetStagedApprenticeQueryResult>(errors);
        mediatorMock
            .Setup(m => m.Send(It.Is<GetStagedApprenticeQuery>(q =>
                q.LastName == lastname
                && q.DateOfBirth == dateofbirth
                && q.Email == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(errorResponse);

        var result = await sut.GetStagedApprentice(lastname, dateofbirth, email);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}