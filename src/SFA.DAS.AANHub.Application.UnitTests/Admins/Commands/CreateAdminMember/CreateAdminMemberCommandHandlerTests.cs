using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Commands.CreateAdminMember;

public class CreateAdminMemberCommandHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_AddsNewAdmin(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        CreateAdminMemberCommandHandler sut,
        CreateAdminMemberCommand command)
    {
        membersReadRepository.Setup(p => p.GetMemberByEmail(command.Email!)).ReturnsAsync((Member)null!);

        var response = await sut.Handle(command, new CancellationToken());
        response.Result.MemberId.Should().Be(command.MemberId);

        membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.MemberId)));
        membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.UserType == MembershipUserType.Admin)));
        auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.MemberId)));
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_ExistingAdmin(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IMembersReadRepository> membersReadRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<IAanDataContext> anDataContext,
        CreateAdminMemberCommandHandler sut,
        CreateAdminMemberCommand command)
    {
        membersReadRepository.Setup(p => p.GetMemberByEmail(command.Email!)).ReturnsAsync(command);

        var response = await sut.Handle(command, new CancellationToken());
        response.Result.MemberId.Should().Be(command.MemberId);

        membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.MemberId)), Times.Never);
        membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.UserType == MembershipUserType.Admin)), Times.Never);
        auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.MemberId)), Times.Never);

        anDataContext.Verify(p => p.SaveChangesAsync(new CancellationToken()), Times.Never);
    }
}