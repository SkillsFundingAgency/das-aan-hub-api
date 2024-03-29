﻿using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberReinstate;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PostMemberReinstate;
public class PostMemberReinstateCommandHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_UpdatesStatusForMember_ReturnsSuccessCommand(
      [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
      Member member,
      PostMemberReinstateCommandHandler sut,
      PostMemberReinstateCommand command)
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
        Member member,
        PostMemberReinstateCommandHandler sut,
        PostMemberReinstateCommand command)
    {
        command.MemberId = member.Id;
        member.UserType = UserType.Apprentice;
        member.Status = "Live";
        membersWriteRepository.Setup(x => x.Get(member.Id)).ReturnsAsync(member);
        await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            membersWriteRepository.Verify(p => p.Get(member.Id));
            auditWriteRepository.Verify(p =>
                p.Create(It.Is<Audit>(
                    x => x.ActionedBy == command.MemberId && x.EntityId == command.MemberId && x.Action == AuditAction.Reinstate)));

            aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_LeavingReasonsDeleted(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IMemberLeavingReasonsWriteRepository> memberLeavingReasonsWriteRepository,
        Member member,
        PostMemberReinstateCommandHandler sut,
        PostMemberReinstateCommand command)
    {
        command.MemberId = member.Id;
        member.UserType = UserType.Apprentice;
        member.Status = "Live";
        membersWriteRepository.Setup(x => x.Get(member.Id)).ReturnsAsync(member);
        await sut.Handle(command, new CancellationToken());

        memberLeavingReasonsWriteRepository.Verify(x => x.DeleteLeavingReasons(member.Id, It.IsAny<CancellationToken>()),
                Times.Once);
    }
}
