using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands
{
    [TestFixture]
    public class MemberTest
    {
        [Test, AutoMoqData]
        public void Apprentice_PopulatesMemberFromCreateApprenticeMemberCommand(CreateApprenticeMemberCommand createApprenticeMemberCommand)
        {
            var command = (Member)createApprenticeMemberCommand;

            Assert.That(command, Is.Not.Null);

            command.Should().BeEquivalentTo(createApprenticeMemberCommand, c => c
                .Excluding(m => m.Joined)
                .Excluding(m => m.Information)
                .Excluding(m => m.ReviewStatus)
                .Excluding(m => m.RequestedByMemberId)
                .Excluding(m => m.Regions)
                .Excluding(m => m.ApprenticeId)
                .Excluding(m => m.Email)
                .Excluding(m => m.Name)
            );

            Assert.AreEqual(createApprenticeMemberCommand.Id, command.Id);
            Assert.AreEqual(createApprenticeMemberCommand.Joined, command.Joined);
            Assert.AreEqual(createApprenticeMemberCommand.Information, command.Information);
        }
    }
}

