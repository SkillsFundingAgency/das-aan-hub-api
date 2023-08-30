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

        existingMember.MemberProfiles = new List<MemberProfile>()
        {   new MemberProfile { ProfileId = 41, ProfileValue = "ToBeUpdated" },
            new MemberProfile { ProfileId = 42, ProfileValue = "NotToBeUpdated" },
            new MemberProfile { ProfileId = 44, ProfileValue = "ToBeSetToNull" }
        };

        var updateProfileModel = new List<UpdateProfileModel>()
        {
            new UpdateProfileModel { ProfileId = 41, Value = "UpdatedValue" },
            new UpdateProfileModel { ProfileId = 43, Value = "ToBeInsertedValue" },
            new UpdateProfileModel { ProfileId = 44, Value = null }
        };

        existingMember.MemberPreferences = new List<MemberPreference>()
        {
            new MemberPreference{ PreferenceId =1, AllowSharing =true },
            new MemberPreference{ PreferenceId =2, AllowSharing =false }
        };

        var updatePreferenceModel = new List<UpdatePreferenceModel>()
        {
            new UpdatePreferenceModel { PreferenceId = 1, Value = false },
            new UpdatePreferenceModel { PreferenceId = 2, Value = true }
        };

        UpdateMemberProfilesCommand command = new(existingMember.Id, existingMember.Id, updateProfileModel, updatePreferenceModel);

        membersWriteRepository.Setup(x => x.Get(existingMember.Id)).ReturnsAsync(() => existingMember);

        await sut.Handle(command, new CancellationToken());

        membersWriteRepository.Verify(p => p.Create(It.IsAny<Member>()), Times.Never);
        aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        var serializedExistingMember = JsonSerializer.Serialize(existingMember);
        auditWriteRepository.Verify(a => a.Create(
                It.Is<Audit>(
                    a => a.Action == "Put"
                    && a.After == serializedExistingMember
                    && a.ActionedBy == command.RequestedByMemberId
                    && a.Resource == nameof(Member))),
                Times.Once);
    }
}
