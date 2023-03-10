using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles;
using SFA.DAS.AANHub.Application.UnitTests;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class ProfilesControllerTests
    {
        [Test]
        [AutoMoqData]
        public async Task GetProfiles_InvokesQueryHandler(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ProfilesController sut)
        {
            string userType = "apprentice";
            var response = new ValidatedResponse<GetProfilesByUserTypeQueryResult>
            (
                new List<ProfileModel>()
            );

            mediatorMock.Setup(m => m.Send(It.Is<GetProfilesByUserTypeQuery>(q => q.UserType == userType), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var getResult = await sut.GetProfilesByUserType(userType);

            var result = getResult as OkObjectResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);

            var queryResult = result.Value as GetProfilesByUserTypeQueryResult;
            queryResult.Should().BeEquivalentTo(response.Result);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetProfilesByUserTypeQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetProfiles_InvokesQueryHandler_NoResultGivesNotFound(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ProfilesController sut)
        {
            string userType = "apprentice";
            var errorResponse = new ValidatedResponse<GetProfilesByUserTypeQueryResult>
                (new List<ValidationFailure>());

            mediatorMock.Setup(m => m.Send(It.Is<GetProfilesByUserTypeQuery>(q => q.UserType == userType), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResponse);

            var response = await sut.GetProfilesByUserType(userType);

            var result = response as NotFoundResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status404NotFound, result!.StatusCode);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetProfilesByUserTypeQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test]
        [AutoMoqData]
        public async Task GetProfiles_InvokesQueryHandler_ResultGivesSuccessfulResult(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ProfilesController sut)
        {
            string userType = "apprentice";
            var response = new ValidatedResponse<GetProfilesByUserTypeQueryResult>
            (
                new List<ProfileModel>()
            );

            mediatorMock.Setup(m => m.Send(It.Is<GetProfilesByUserTypeQuery>(q => q.UserType == userType), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var getResult = await sut.GetProfilesByUserType(userType);

            var result = getResult as OkObjectResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        }

        [Test]
        [AutoMoqData]
        public async Task GetProfiles_InvokesQueryHandler_BadResultGivesBadRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ProfilesController sut)
        {
            string userType = "apprentice";
            var errorResponse = new ValidatedResponse<GetProfilesByUserTypeQueryResult>
            (new List<ValidationFailure>
            {
                new("Name", "error")
            });

            mediatorMock.Setup(m => m.Send(It.Is<GetProfilesByUserTypeQuery>(q => q.UserType == userType), It.IsAny<CancellationToken>()))
                .ReturnsAsync(errorResponse);

            var response = await sut.GetProfilesByUserType(userType);

            var result = response as BadRequestObjectResult;
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
}
