using System.Text.Json;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.MemberProfiles.Commands.PutMemberProfile;
public class UpdateMemberProfilesCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_NoMatchingMemberUpdatesNothing(
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        UpdateMemberProfilesCommandHandler sut)
    {
        Member existingMember = new();
        existingMember.Id = Guid.NewGuid();
        UpdateMemberProfilesCommand command = new(existingMember.Id, existingMember.Id, new List<UpdateProfileModel>(), new List<UpdatePreferenceModel>());
        membersWriteRepository.Setup(x => x.Get(existingMember.Id)).ReturnsAsync(() => null);

        await sut.Handle(command, new CancellationToken());

        membersWriteRepository.Verify(p => p.Create(It.IsAny<Member>()), Times.Never);
        auditWriteRepository.Verify(p => p.Create(It.IsAny<Audit>()), Times.Never);
        aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_MatchingMemberUpdatesProfilesAndPreferences(
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        UpdateMemberProfilesCommandHandler sut)
    {
        Member existingMember = new();
        existingMember.Id = Guid.NewGuid();
        UpdateMemberProfilesCommand command = new(existingMember.Id, existingMember.Id, new List<UpdateProfileModel>(), new List<UpdatePreferenceModel>());
        membersWriteRepository.Setup(x => x.Get(existingMember.Id)).ReturnsAsync(() => existingMember);

        await sut.Handle(command, new CancellationToken());

        membersWriteRepository.Verify(p => p.Create(It.IsAny<Member>()), Times.Never);
        aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        var serializedExistingMember = JsonSerializer.Serialize(existingMember);
        auditWriteRepository.Verify(a => a.Create(
                It.Is<Audit>(
                    a => a.Action == "Put"
                    && a.Before == serializedExistingMember
                    && a.ActionedBy == command.RequestedByMemberId
                    && a.Resource == nameof(Member))),
                Times.Once);
    }
}
