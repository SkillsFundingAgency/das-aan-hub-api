using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberRemove;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PostMemberStatus;
public class PostMemberRemoveCommandHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_UpdatesStatusForMember_ReturnsSuccessCommand(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        Member member,
        PostMemberRemoveCommandHandler sut,
        PostMemberRemoveCommand command)
    {
        command.MemberId = member.Id;
        membersWriteRepository.Setup(x => x.Get(member.Id)).ReturnsAsync(member);

        var result = await sut.Handle(command, new CancellationToken());

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValidResponse, Is.EqualTo(true));
            Assert.That(result.Result, Is.InstanceOf<SuccessCommandResult>());
        });
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_UpdatesStatusForMember(
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        Member member,
        PostMemberRemoveCommandHandler sut,
        PostMemberRemoveCommand command)
    {
        command.MemberId = member.Id;
        membersWriteRepository.Setup(x => x.Get(member.Id)).ReturnsAsync(member);

        await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            membersWriteRepository.Verify(p => p.Get(member.Id));
            auditWriteRepository.Verify(p =>
                p.Create(It.Is<Audit>(
                    x => x.ActionedBy == command.AdminMemberId && x.EntityId == command.MemberId)));
            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_UpdatesStatusForMember_Removed_NotificationCalled(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationsWriteRepository,
        Member member,
        PostMemberRemoveCommandHandler sut,
        PostMemberRemoveCommand command)
    {
        command.MemberId = member.Id;
        command.Status = MembershipStatusType.Removed;
        membersWriteRepository.Setup(x => x.Get(member.Id)).ReturnsAsync(member);

        await sut.Handle(command, new CancellationToken());

        var tokenDictionary = new Dictionary<string, string>
        {
            { "contact", member.FullName },
        };

        var tokens = JsonSerializer.Serialize(tokenDictionary);

        notificationsWriteRepository.Verify(p =>
            p.Create(It.Is<Notification>(x =>
                x.MemberId == command.AdminMemberId && x.ReferenceId == command.MemberId.ToString() &&
                x.Tokens == tokens)), Times.Once);
    }


    [Test, RecursiveMoqAutoData]
    public async Task Handle_UpdatesStatusForMember_Deleted_NotificationNotCalled(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationsWriteRepository,
        Member member,
        PostMemberRemoveCommandHandler sut,
        PostMemberRemoveCommand command)
    {
        command.MemberId = member.Id;
        command.Status = MembershipStatusType.Deleted;
        membersWriteRepository.Setup(x => x.Get(member.Id)).ReturnsAsync(member);

        await sut.Handle(command, new CancellationToken());

        notificationsWriteRepository.Verify(p =>
                p.Create(It.IsAny<Notification>()), Times.Never);

    }
}
