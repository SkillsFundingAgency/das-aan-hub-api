using System.Text.Json;
using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberLeaving;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PostMemberLeaving;
public class PostMemberLeavingCommandHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_UpdatesStatusForMember_ReturnsSuccessCommand(
       [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
       Member member,
       PostMemberLeavingCommandHandler sut,
       PostMemberLeavingCommand command)
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
    public async Task Handle_AuditCalled(
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<ILeavingReasonsReadRepository> leavingReasonsReadRepository,
        List<int> leavingReasonIds,
        Member member,
        PostMemberLeavingCommandHandler sut,
        PostMemberLeavingCommand command)
    {
        command.MemberId = member.Id;
        member.UserType = UserType.Apprentice;
        member.Status = "Live";
        membersWriteRepository.Setup(x => x.Get(member.Id)).ReturnsAsync(member);
        var leavingReasons = leavingReasonIds.Distinct().Select(id => new LeavingReason { Category = "Category", Description = "desc " + id.ToString(), Id = id, Ordering = id }).ToList();

        leavingReasonsReadRepository.Setup(x => x.GetAllLeavingReasons(It.IsAny<CancellationToken>()))
            .ReturnsAsync(leavingReasons);

        await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            membersWriteRepository.Verify(p => p.Get(member.Id));
            auditWriteRepository.Verify(p =>
                p.Create(It.Is<Audit>(
                    x => x.ActionedBy == command.MemberId && x.EntityId == command.MemberId && x.Action == AuditAction.Withdrawn)));

            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_WriteLeavingReasonsCalled(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<ILeavingReasonsReadRepository> leavingReasonsReadRepository,
        [Frozen] Mock<IMemberLeavingReasonsWriteRepository> memberLeavingReasonsWriteRepository,
        List<int> leavingReasonIds,
        Member member,
        PostMemberLeavingCommandHandler sut,
        PostMemberLeavingCommand command)
    {
        command.MemberId = member.Id;
        member.UserType = UserType.Apprentice;
        member.Status = "Live";
        membersWriteRepository.Setup(x => x.Get(member.Id)).ReturnsAsync(member);
        var leavingReasons = leavingReasonIds.Distinct().Select(id => new LeavingReason { Category = "Category", Description = "desc " + id.ToString(), Id = id, Ordering = id }).ToList();

        command.LeavingReasons = leavingReasonIds.Distinct().ToList();

        leavingReasonsReadRepository.Setup(x => x.GetAllLeavingReasons(It.IsAny<CancellationToken>()))
            .ReturnsAsync(leavingReasons);

        await sut.Handle(command, new CancellationToken());

        memberLeavingReasonsWriteRepository.Verify(x => x.CreateMemberLeavingReasons(
                    It.IsAny<List<MemberLeavingReason>>()),
                Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_ApprenticeNotificationCalled(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationsWriteRepository,
        Member member,
        List<int> leavingReasons,
        PostMemberLeavingCommandHandler sut,
        PostMemberLeavingCommand command)
    {
        command.MemberId = member.Id;
        command.LeavingReasons = leavingReasons;
        member.UserType = UserType.Apprentice;
        membersWriteRepository.Setup(x => x.Get(member.Id)).ReturnsAsync(member);

        await sut.Handle(command, new CancellationToken());

        var tokenDictionary = new Dictionary<string, string>
        {
            { "contact", member.FullName },
        };

        var tokens = JsonSerializer.Serialize(tokenDictionary);

        notificationsWriteRepository.Verify(p =>
            p.Create(It.Is<Notification>(x =>
                x.MemberId == command.MemberId && x.ReferenceId == command.MemberId.ToString() &&
                x.TemplateName == Domain.Common.Constants.EmailTemplateName.ApprenticeWithdrawal &&
                x.Tokens == tokens)), Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_EmployerNotificationCalled(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationsWriteRepository,
        Member member,
        List<int> leavingReasons,
        PostMemberLeavingCommandHandler sut,
        PostMemberLeavingCommand command)
    {
        command.MemberId = member.Id;
        command.LeavingReasons = leavingReasons;
        member.UserType = UserType.Employer;
        membersWriteRepository.Setup(x => x.Get(member.Id)).ReturnsAsync(member);

        await sut.Handle(command, new CancellationToken());

        var tokenDictionary = new Dictionary<string, string>
        {
            { "contact", member.FullName },
        };

        var tokens = JsonSerializer.Serialize(tokenDictionary);

        notificationsWriteRepository.Verify(p =>
            p.Create(It.Is<Notification>(x =>
                x.MemberId == command.MemberId && x.ReferenceId == command.MemberId.ToString() &&
                x.TemplateName == Domain.Common.Constants.EmailTemplateName.EmployerWithdrawal &&
                x.Tokens == tokens)), Times.Once);
    }
}
