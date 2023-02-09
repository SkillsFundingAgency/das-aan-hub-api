using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Profiles.Queries
{
    [TestFixture]
    public class GetProfilesQueryHandlerTests
    {
        [Test]
        public async Task Handle_GetProfilesQuery()
        {
            string userType = "Apprentice";
            int profileId = 1;

            var profileReadReposotoryMock = new Mock<IProfilesReadRepository>();
            var profiles = new List<Profile>() { new Profile { Id = profileId } };

            profileReadReposotoryMock.Setup(x => x.GetAllProfiles(userType)).ReturnsAsync(profiles);

            var sut = new GetProfilesQueryHandler(profileReadReposotoryMock.Object);

            var result = await sut.Handle(new GetProfilesQuery(userType), new CancellationToken());

            result.Result.As<List<ProfileModel>>().Should().NotBeNullOrEmpty();
            Assert.That(result.Result.Any(x => x.Id == profileId));
        }
    }
}
