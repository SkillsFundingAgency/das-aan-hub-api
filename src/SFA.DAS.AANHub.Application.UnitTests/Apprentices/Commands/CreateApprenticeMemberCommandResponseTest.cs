using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands
{
    [TestFixture]
    public class CreateApprenticeMemberCommandResponseTest
    {
        [Test]
        [AutoMoqData]
        public void Apprentice_PopulatesMemberFromCreateApprenticeMemberCommand(Member member)
        {
            var response = (CreateApprenticeMemberCommandResponse)member;

            Assert.That(response, Is.Not.Null);

            Assert.AreEqual(member.Id, response.MemberId);
            Assert.AreEqual(member.Status, response.Status);
        }
    }
}