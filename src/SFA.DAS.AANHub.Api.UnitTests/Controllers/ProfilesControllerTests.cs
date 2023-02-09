using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ProfilesController _controller;
        private readonly Mock<IMediator> _mediator;

        public ProfilesControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _controller = new ProfilesController(Mock.Of<ILogger<ProfilesController>>(), _mediator.Object);
        }

        [Test]
        [AutoMoqData]
        public async Task GetProfiles_InvokesQueryHandler_ReturnsProfiles(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ProfilesController sut)
        {
            string userType = "Apprentice";
            var response = new ValidatedResponse<List<ProfileModel>>
            (
                new List<ProfileModel>()
            );

            mediatorMock.Setup(m => m.Send(It.Is<GetProfilesQuery>(q => q.UserType == userType), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var getResult = await sut.GetProfiles(userType);

            var result = getResult as OkObjectResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        }
    }
}
