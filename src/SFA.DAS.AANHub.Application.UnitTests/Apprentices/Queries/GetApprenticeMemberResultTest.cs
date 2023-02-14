using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Queries;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Queries
{
    [TestFixture]
    public class GetApprenticeMemberResultTest
    {
        [Test, AutoMoqData]
        public void Apprentice_PopulatesGetApprenticeMemberResultFromApprentice(Apprentice apprentice)
        {
            var response = (GetApprenticeMemberResult?)apprentice;

            Assert.That(response, Is.Not.Null);

            Assert.AreEqual(apprentice.MemberId, response!.MemberId);
            Assert.AreEqual(apprentice.Name, response.Name);
            Assert.AreEqual(apprentice.Email, response.Email);
            Assert.AreEqual(apprentice.Member.Status, response.Status);


        }

        [Test, AutoMoqData]
        public void Apprentice_GetApprenticeMemberResultTest_ApprenticeIsNull()
        {
            Apprentice? apprentice = null;
            var response = (GetApprenticeMemberResult?)apprentice!;
            Assert.That(response, Is.Null);
        }
    }
}
