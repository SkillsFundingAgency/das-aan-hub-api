using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PatchMember;

public class PatchMemberCommandHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_DataFound_Patch(
    [Frozen] Mock<IMembersWriteRepository> memberWriteRepoMock,
    [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
    [Frozen] Mock<IAanDataContext> dataContextMock,
    PatchMemberCommandHandler sut,
    PatchMemberCommand command,
    Member member,
    CancellationToken cancellationToken)
    {
        memberWriteRepoMock.Setup(r => r.Get(command.MemberId)).ReturnsAsync(member);

        var patchDoc = new JsonPatchDocument<Member>();
        patchDoc.Replace(path => path.Email, member.Email);
        command.PatchDoc = patchDoc;

        var response = await sut.Handle(command, cancellationToken);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();
            response.Result.IsSuccess.Should().BeTrue();
            response.IsValidResponse.Should().BeTrue();
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.Action == "PatchMember" && x.ActionedBy == command.MemberId && x.Resource == "Member")));
            dataContextMock.Verify(d => d.SaveChangesAsync(cancellationToken), Times.Once);
        }
    }

    [Test, MoqAutoData]
    public async Task Handle_NoDataFound(
        [Frozen] Mock<IMembersWriteRepository> memberWriteRepoMock,
        PatchMemberCommandHandler sut,
        PatchMemberCommand command,
        CancellationToken cancellationToken)
    {
        memberWriteRepoMock.Setup(r => r.Get(command.MemberId)).ReturnsAsync(() => null);

        var response = await sut.Handle(command, cancellationToken);

        Assert.IsFalse(response.Result.IsSuccess);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_StatusIsWithdrawnAndMemberIsNotApprenticeOrEmployer_ThrowNotImplementedException(
    [Frozen] Mock<IMembersWriteRepository> memberWriteRepoMock,
    [Frozen] Mock<IAanDataContext> dataContextMock,
    PatchMemberCommandHandler sut,
    PatchMemberCommand command,
    Member member,
    CancellationToken cancellationToken)
    {
        member.UserType = UserType.Admin;
        memberWriteRepoMock.Setup(r => r.Get(command.MemberId)).ReturnsAsync(member);

        var patchDoc = new JsonPatchDocument<Member>();
        patchDoc.Replace(path => path.Status, Constants.MembershipStatus.Withdrawn);
        command.PatchDoc = patchDoc;

        Func<Task> response = async () => await sut.Handle(command, cancellationToken);

        using (new AssertionScope())
        {
            await response.Should().ThrowAsync<NotImplementedException>();
            dataContextMock.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_StatusIsWithdrawnAndMemberIsEmployer_CorrectEmailTemplateInNotification(
        [Frozen] Mock<IMembersWriteRepository> memberWriteRepoMock,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationWriteRepositoryMock,
        [Frozen] Mock<IAanDataContext> dataContextMock,
        PatchMemberCommandHandler sut,
        PatchMemberCommand command,
        Member member,
        CancellationToken cancellationToken)
    {
        member.UserType = UserType.Employer;
        memberWriteRepoMock.Setup(r => r.Get(command.MemberId)).ReturnsAsync(member);

        var patchDoc = new JsonPatchDocument<Member>();
        patchDoc.Replace(path => path.Status, Constants.MembershipStatus.Withdrawn);
        command.PatchDoc = patchDoc;

        var response = await sut.Handle(command, cancellationToken);

        using (new AssertionScope())
        {
            notificationWriteRepositoryMock.Verify(n => n.Create(It.Is<Notification>(n => n.TemplateName == EmailTemplateName.EmployerWithdrawal)), Times.Once());
            dataContextMock.Verify(d => d.SaveChangesAsync(cancellationToken), Times.Once);
            Assert.NotNull(response);
            response.Result.IsSuccess.Should().BeTrue();
            response.IsValidResponse.Should().BeTrue();
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.Action == "PatchMember" && x.ActionedBy == command.MemberId && x.Resource == "Member")));
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_StatusIsWithdrawnAndMemberIsApprentice_CorrectEmailTemplateInNotification(
        [Frozen] Mock<IMembersWriteRepository> memberWriteRepoMock,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationWriteRepositoryMock,
        [Frozen] Mock<IAanDataContext> dataContextMock,
        PatchMemberCommandHandler sut,
        PatchMemberCommand command,
        Member member,
        CancellationToken cancellationToken)
    {
        member.UserType = UserType.Apprentice;
        memberWriteRepoMock.Setup(r => r.Get(command.MemberId)).ReturnsAsync(member);

        var patchDoc = new JsonPatchDocument<Member>();
        patchDoc.Replace(path => path.Status, Constants.MembershipStatus.Withdrawn);
        command.PatchDoc = patchDoc;

        var response = await sut.Handle(command, cancellationToken);

        using (new AssertionScope())
        {
            notificationWriteRepositoryMock.Verify(n => n.Create(It.Is<Notification>(n => n.TemplateName == EmailTemplateName.ApprenticeWithdrawal)), Times.Once());
            dataContextMock.Verify(d => d.SaveChangesAsync(cancellationToken), Times.Once);
            Assert.NotNull(response);
            response.Result.IsSuccess.Should().BeTrue();
            response.IsValidResponse.Should().BeTrue();
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.Action == "PatchMember" && x.ActionedBy == command.MemberId && x.Resource == "Member")));
        }
    }

    [Test, RecursiveMoqInlineAutoData(UserType.Apprentice)]
    [RecursiveMoqInlineAutoData(UserType.Employer)]
    public async Task Handle_StatusIsWithdrawnAndMemberIsFound_NotificationIsCreated(
        UserType userType,
        [Frozen] Mock<IMembersWriteRepository> memberWriteRepoMock,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationWriteRepositoryMock,
        [Frozen] Mock<IAanDataContext> dataContextMock,
        PatchMemberCommandHandler sut,
        PatchMemberCommand command,
        Member member,
        CancellationToken cancellationToken)
    {
        member.UserType = userType;
        memberWriteRepoMock.Setup(r => r.Get(command.MemberId)).ReturnsAsync(member);

        var patchDoc = new JsonPatchDocument<Member>();
        patchDoc.Replace(path => path.Status, Constants.MembershipStatus.Withdrawn);
        command.PatchDoc = patchDoc;

        var response = await sut.Handle(command, cancellationToken);

        using (new AssertionScope())
        {
            notificationWriteRepositoryMock.Verify(n => n.Create(It.IsAny<Notification>()), Times.Once());
            dataContextMock.Verify(d => d.SaveChangesAsync(cancellationToken), Times.Once);
            Assert.NotNull(response);
            response.Result.IsSuccess.Should().BeTrue();
            response.IsValidResponse.Should().BeTrue();
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.Action == "PatchMember" && x.ActionedBy == command.MemberId && x.Resource == "Member")));
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_StatusIsDeletedAndMemberIsFound_NotificationNotCreated(
        [Frozen] Mock<IMembersWriteRepository> memberWriteRepoMock,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationWriteRepositoryMock,
        [Frozen] Mock<IAanDataContext> dataContextMock,
        PatchMemberCommandHandler sut,
        PatchMemberCommand command,
        Member member,
        CancellationToken cancellationToken)
    {
        memberWriteRepoMock.Setup(r => r.Get(command.MemberId)).ReturnsAsync(member);

        var patchDoc = new JsonPatchDocument<Member>();
        patchDoc.Replace(path => path.Status, Constants.MembershipStatus.Deleted);
        command.PatchDoc = patchDoc;

        var response = await sut.Handle(command, cancellationToken);

        using (new AssertionScope())
        {
            notificationWriteRepositoryMock.Verify(n => n.Create(It.IsAny<Notification>()), Times.Never());
            dataContextMock.Verify(d => d.SaveChangesAsync(cancellationToken), Times.Once);
            Assert.NotNull(response);
            response.Result.IsSuccess.Should().BeTrue();
            response.IsValidResponse.Should().BeTrue();
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.Action == "PatchMember" && x.ActionedBy == command.MemberId && x.Resource == "Member")));
        }
    }
}
