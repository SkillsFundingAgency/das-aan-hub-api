using NUnit.Framework;
using SFA.DAS.AANHub.Application.Partners.Queries;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Queries
{
    [TestFixture]
    public class GetPartnerMemberResultTest
    {
        [Test, AutoMoqData]
        public void Partner_PopulatesGetPartnerMemberResultFromPartner(Partner partner)
        {
            var response = (GetPartnerMemberResult?)partner;

            Assert.That(response, Is.Not.Null);

            Assert.AreEqual(partner.MemberId, response!.MemberId);
            Assert.AreEqual(partner.Organisation, response.Organisation);
            Assert.AreEqual(partner.Email, response.Email);

        }

        [Test, AutoMoqData]
        public void Partner_GetPartnerMemberResultTest_PartnerIsNull()
        {
            Partner? partner = null;
            var response = (GetPartnerMemberResult?)partner!;
            Assert.That(response, Is.Null);
        }
    }
}
