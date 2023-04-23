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
using SFA.DAS.AANHub.Application.Partners.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;

public class PartnersControllerGetTests
{
    [Test]
    [MoqAutoData]
    public async Task GetPartner_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PartnersController sut,
        string userName)
    {
        await sut.GetPartner(userName);

        mediatorMock.Verify(m => m.Send(new GetPartnerMemberQuery(userName), It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task GetPartner_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PartnersController sut,
        string userName)
    {
        var notFoundResponse = ValidatedResponse<GetMemberResult>.EmptySuccessResponse();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetPartnerMemberQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(notFoundResponse);

        var result = await sut.GetPartner(userName);

        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task GetPartner_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PartnersController sut,
        string userName,
        GetMemberResult getMemberResult)
    {
        var response = new ValidatedResponse<GetMemberResult>(getMemberResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetPartnerMemberQuery>(q => q.UserName == userName), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await sut.GetPartner(userName);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getMemberResult);
    }

    [Test]
    [MoqAutoData]
    public async Task GetPartner_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PartnersController sut,
        string userName,
        List<ValidationFailure> errors)

    {
        var response = new ValidatedResponse<GetMemberResult>(errors);
        mediatorMock.Setup(m => m.Send(It.IsAny<GetPartnerMemberQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await sut.GetPartner(userName);

        result.As<BadRequestObjectResult>().StatusCode.Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}