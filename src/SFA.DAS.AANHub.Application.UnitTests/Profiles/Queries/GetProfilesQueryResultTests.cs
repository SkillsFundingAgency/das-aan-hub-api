namespace SFA.DAS.AANHub.Application.UnitTests.Profiles.Queries
{
    [TestFixture]
    public class GetProfilesQueryResultTests
    {
        [Test, AutoMoqData]
        public void Profiles_PopulatesGetProfilesResultFromProfile(List<Profile> profiles)
        {
            var response = (GetProfilesQueryResult?)profiles;

            Assert.That(response, Is.Not.Null);

            Assert.AreEqual(profiles.Count, response!.Profiles.Count);
            //Assert.AreEqual(profiles.Description, response.Description);
            //Assert.AreEqual(profiles.Category, response.Category);
            //Assert.AreEqual(profiles.Ordering, response.Ordering);
            //Assert.AreEqual(profiles.UserType, response.UserType);
        }

        [Test, AutoMoqData]
        public void Profile_GetProfileQueryResultTest_ProfileIsNull()
        {
            List<Profile>? profile = null;
            var response = (GetProfilesQueryResult?)profile!;
            Assert.That(response, Is.Null);
        }
    }
}
