using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands;

public class MemberTest
{
    [Test]
    [MoqAutoData]
    public void Apprentice_PopulatesMemberFromCreateApprenticeMemberCommand(CreateApprenticeMemberCommand createApprenticeMemberCommand)
    {
        var command = (Member)createApprenticeMemberCommand;

        Assert.That(command, Is.Not.Null);

        Assert.AreEqual(createApprenticeMemberCommand.Id, command.Id);
        Assert.AreEqual(createApprenticeMemberCommand.Joined, command.Joined);
        Assert.AreEqual(createApprenticeMemberCommand.Information, command.Information);
    }
}