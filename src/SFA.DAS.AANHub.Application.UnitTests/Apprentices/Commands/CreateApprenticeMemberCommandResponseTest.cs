using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands
{
    [TestFixture]
    public class CreateApprenticeMemberCommandResponseTest
    {
        [Test, AutoMoqData]
        public void Apprentice_PopulatesMemberFromCreateApprenticeMemberCommand(Member member)
        {
            var response = (CreateApprenticeMemberCommandResponse)member;

            Assert.That(response, Is.Not.Null);

            response.Should().BeEquivalentTo(member, m => m
                .Excluding(r => r.Id)
                .Excluding(r => r.UserType)
                .Excluding(r => r.ReviewStatus)
                .Excluding(r => r.Information)
                .Excluding(r => r.Joined)
                .Excluding(r => r.Created)
                .Excluding(r => r.Deleted)
                .Excluding(r => r.Updated)
                .Excluding(r => r.Admin)
                .Excluding(r => r.Apprentice)
                .Excluding(r => r.Employer)
                .Excluding(r => r.Partner)
                .Excluding(r => r.MemberRegions)
            );

            Assert.AreEqual(member.Id, response.MemberId);
            Assert.AreEqual(member.Status, response.Status);
        }

    }
}
