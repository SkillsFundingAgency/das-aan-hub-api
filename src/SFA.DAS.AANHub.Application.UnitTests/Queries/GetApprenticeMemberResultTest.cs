using NUnit.Framework;
using SFA.DAS.AANHub.Application.Queries.GetApprentice;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands
{
    [TestFixture]
    public class GetApprenticeMemberResultTest
    {
        [Test, AutoMoqData]
        public void Apprentice_PopulatesGetApprenticeMemberResultFromApprentice(Apprentice apprentice)
        {
            var response = (GetApprenticeMemberResult)apprentice;

            Assert.That(response, Is.Not.Null);

            Assert.AreEqual(apprentice.MemberId, response.MemberId);
            Assert.AreEqual(apprentice.Name, response.Name);
            Assert.AreEqual(apprentice.Email, response.Email);
        }

    }
}
