using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Profiles.Queries;

[TestFixture]
public class ProfileModelTests
{
    [Test]
    [AutoData]
    public void ProfileModelTest_ReturnsExpectedFields(Profile profile)
    {
        ProfileModel profileModel = (ProfileModel)profile;

        profileModel.Id.Should().Be(profile.Id);
        profileModel.Category.Should().Be(profile.Category);
        profileModel.Description.Should().Be(profile.Description);
        profileModel.Ordering.Should().Be(profile.Ordering);
    }
}
