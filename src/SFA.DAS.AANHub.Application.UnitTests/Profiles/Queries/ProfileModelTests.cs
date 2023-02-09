using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Profiles.Queries
{
    [TestFixture]
    public class ProfileModelTests
    {
        [Test]
        public void ProfileModelTest_ReturnsExpectedFields()
        {
            const string category = "Events";
            const string description = "Networking at events in person";
            const string userType = "Apprentice";
            const int id = 1;
            const int ordering = 2;

            var profile = new Profile
            {
                Id = id,
                Category = category,
                Description = description,
                UserType = userType,
                Ordering = ordering
            };

            ProfileModel profileModel = (ProfileModel)profile;

            profileModel.Id.Should().Be(id);
            profileModel.Category.Should().Be(category);
            profileModel.Description.Should().Be(description);
            profileModel.Ordering.Should().Be(ordering);
        }
    }
}
