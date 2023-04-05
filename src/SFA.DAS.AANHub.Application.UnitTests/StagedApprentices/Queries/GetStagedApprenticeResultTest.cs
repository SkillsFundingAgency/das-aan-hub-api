using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles;
using SFA.DAS.AANHub.Application.StagedApprentices.Queries;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.StagedApprentices.Queries
{
    [TestFixture]
    public class GetStagedApprenticeResultTest
    {
        [Test, AutoMoqData]
        public void StagedApprentice_ReturnsExpectedFields(StagedApprentice stagedApprentice)
        {
            var response = (GetStagedApprenticeResult?)stagedApprentice;

            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);

                Assert.AreEqual(stagedApprentice.Uln, response!.Uln);
                Assert.AreEqual(stagedApprentice.ApprenticeshipId, response.ApprenticeshipId);
                Assert.AreEqual(stagedApprentice.EmployerName, response.EmployerName);
                Assert.AreEqual(stagedApprentice.StartDate, response.StartDate);
                Assert.AreEqual(stagedApprentice.EndDate, response.EndDate);
                Assert.AreEqual(stagedApprentice.TrainingProviderId, response.TrainingProviderId);
                Assert.AreEqual(stagedApprentice.TrainingProviderName, response.TrainingProviderName);
                Assert.AreEqual(stagedApprentice.TrainingCode, response.TrainingCode);
                Assert.AreEqual(stagedApprentice.StandardUId, response.StandardUId);
            });
        }

        [Test, AutoMoqData]
        public void StagedApprentice_GetStagedApprentice_StagedApprenticeIsNull()
        {
            StagedApprentice? stagedApprentice = null;
            var response = (GetStagedApprenticeResult?)stagedApprentice!;
            Assert.That(response, Is.Null);
        }

        //[Test]
        //[AutoData]
        //public void StagedApprentice_StagedApprenticeNotFound_ThrowsInvalidOperationException(OnboardingSessionModel sut, ProfileModel profileModel)
        //{
        //    StagedApprentice? stagedApprentice = null;

        //    Action action = () => sut.GetStagedApprenticeResult(profileModel.Id);

        //    action.Should().Throw<InvalidOperationException>();
        //}
    }
}
