using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;

public class ProfilesControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task GetProfiles_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProfilesController sut)
    {
        var userType = UserType.Apprentice;
        var response = new GetProfilesByUserTypeQueryResult();

        mediatorMock.Setup(m => m.Send(It.Is<GetProfilesByUserTypeQuery>(q => q.UserType == userType), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var getResult = await sut.GetProfilesByUserType(userType);

        var result = getResult as OkObjectResult;
        result.Should().NotBeNull();
        Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);

        var queryResult = result.Value as GetProfilesByUserTypeQueryResult;
        queryResult.Should().BeEquivalentTo(response);

        mediatorMock.Verify(m => m.Send(It.IsAny<GetProfilesByUserTypeQuery>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task GetProfiles_InvokesQueryHandler_ResultGivesSuccessfulResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProfilesController sut,
        GetProfilesByUserTypeQueryResult expectedResult)
    {
        var userType = UserType.Apprentice;
        var response = new ValidatedResponse<GetProfilesByUserTypeQueryResult>
        (
            new List<ProfileModel>()
        );

        mediatorMock.Setup(m => m.Send(It.Is<GetProfilesByUserTypeQuery>(q => q.UserType == userType), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var getResult = await sut.GetProfilesByUserType(userType);

        var result = getResult as OkObjectResult;
        result.Should().NotBeNull();
        Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
    }
}
