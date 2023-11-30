using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.MemberProfiles.Commands.PutMemberProfile;
public class UpdateMemberProfilesCommandHandlerTests
{
    private static List<MemberProfile> memberProfiles = new List<MemberProfile>()
        {   new MemberProfile { ProfileId = 41, ProfileValue = "ToBeUpdated" },
            new MemberProfile { ProfileId = 42, ProfileValue = "NotToBeUpdated" },
            new MemberProfile { ProfileId = 44, ProfileValue = "ToBeSetToNull" }
        };

    private static List<UpdateProfileModel> updateProfileModels = new List<UpdateProfileModel>()
        {
            new UpdateProfileModel { MemberProfileId = 41, Value = "UpdatedValue" },
            new UpdateProfileModel { MemberProfileId = 42, Value = "ToBeInsertedValue" },
            new UpdateProfileModel { MemberProfileId = 44, Value = null },
            new UpdateProfileModel { MemberProfileId = 45, Value = "ToBeNotNull" }
        };

    private static List<MemberPreference> memberPreferences = new List<MemberPreference>()
        {
            new MemberPreference{ PreferenceId =1, AllowSharing =true },
            new MemberPreference{ PreferenceId =2, AllowSharing =false }
        };

    private static List<UpdatePreferenceModel> updatePreferenceModels = new List<UpdatePreferenceModel>()
        {
            new UpdatePreferenceModel { PreferenceId = 1, Value = false },
            new UpdatePreferenceModel { PreferenceId = 2, Value = true }
        };
    private static UpdateMemberProfilesCommand command = null!;
    private static Member existingMember = new();

    [Test, MoqAutoData]
    public async Task Handle_ShouldInvokeSaveChangesAsync(
      [Frozen] Mock<IAanDataContext> aanDataContext,
      [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
      UpdateMemberProfilesCommandHandler sut)
    {
        // Arrange
        existingMember.Id = Guid.NewGuid();
        existingMember.MemberProfiles = memberProfiles;
        existingMember.MemberPreferences = memberPreferences;
        membersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(() => existingMember);
        command = new(existingMember.Id, updateProfileModels, updatePreferenceModels);

        // Act
        await sut.Handle(command, new CancellationToken());

        // Assert
        aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    public static async Task Handle_ShouldInvokeGet(
      [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
      UpdateMemberProfilesCommandHandler sut)
    {
        // Arrange
        existingMember.Id = Guid.NewGuid();
        existingMember.MemberProfiles = memberProfiles;
        existingMember.MemberPreferences = memberPreferences;
        membersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(() => existingMember);
        command = new(existingMember.Id, updateProfileModels, updatePreferenceModels);

        // Act
        await sut.Handle(command, new CancellationToken());

        // Assert
        membersWriteRepository.Verify(a => a.Get(It.IsAny<Guid>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_MemberPreferencesIsEmpty_CreateShouldNotBeInvokedForMemberPreferences(
          [Frozen] Mock<IAanDataContext> aanDataContext,
          [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
          [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
          UpdateMemberProfilesCommandHandler sut)
    {
        // Arrange
        existingMember.Id = Guid.NewGuid();
        existingMember.MemberProfiles = memberProfiles;
        existingMember.MemberPreferences = memberPreferences;
        membersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(() => existingMember);
        command = new(existingMember.Id, updateProfileModels, updatePreferenceModels);
        command.MemberPreferences = new List<UpdatePreferenceModel>();

        // Act
        await sut.Handle(command, new CancellationToken());

        // Assert
        auditWriteRepository.Verify(a => a.Create(
        It.Is<Audit>(
            a => a.Action == "Put"
            && a.Resource == nameof(MemberPreference))),
        Times.Never);
        auditWriteRepository.Verify(a => a.Create(
        It.Is<Audit>(
            a => a.Action == "Put"
            && a.Resource == nameof(MemberProfile))),
        Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_MemberProfilesIsEmpty_CreateShouldNotBeInvokedForMemberProfile(
      [Frozen] Mock<IAanDataContext> aanDataContext,
      [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
      [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
      UpdateMemberProfilesCommandHandler sut)
    {
        // Arrange
        existingMember.Id = Guid.NewGuid();
        existingMember.MemberProfiles = memberProfiles;
        existingMember.MemberPreferences = memberPreferences;
        membersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(() => existingMember);
        command = new(existingMember.Id, updateProfileModels, updatePreferenceModels);
        command.MemberProfiles = new List<UpdateProfileModel>();

        // Act
        await sut.Handle(command, new CancellationToken());

        // Assert
        auditWriteRepository.Verify(a => a.Create(
        It.Is<Audit>(
            a => a.Action == "Put"
            && a.Resource == nameof(MemberProfile))),
        Times.Never);
        auditWriteRepository.Verify(a => a.Create(
        It.Is<Audit>(
            a => a.Action == "Put"
            && a.Resource == nameof(MemberPreference))),
        Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_MemberProfilesAndMemberPreferencesAreEmpty_CreateShouldNotBeInvoked([Frozen] Mock<IAanDataContext> aanDataContext,
      [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
      [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
      UpdateMemberProfilesCommandHandler sut)
    {
        // Arrange
        existingMember.Id = Guid.NewGuid();
        existingMember.MemberProfiles = memberProfiles;
        existingMember.MemberPreferences = memberPreferences;
        membersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(() => existingMember);
        command = new(existingMember.Id, updateProfileModels, updatePreferenceModels);
        command.MemberProfiles = new List<UpdateProfileModel>();
        command.MemberPreferences = new List<UpdatePreferenceModel>();

        // Act
        await sut.Handle(command, new CancellationToken());

        // Assert
        auditWriteRepository.Verify(a => a.Create(
        It.Is<Audit>(
            a => a.Action == "Put"
            && a.Resource == nameof(MemberProfile))),
        Times.Never);
        auditWriteRepository.Verify(a => a.Create(
        It.Is<Audit>(
            a => a.Action == "Put"
            && a.Resource == nameof(MemberPreference))),
        Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handle_OnHandlerSuccess_ReturnsSuccessCommandResult(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        UpdateMemberProfilesCommandHandler sut)
    {
        // Arrange
        existingMember.Id = Guid.NewGuid();
        existingMember.MemberProfiles = memberProfiles;
        existingMember.MemberPreferences = memberPreferences;
        membersWriteRepository.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(() => existingMember);
        command = new(existingMember.Id, updateProfileModels, updatePreferenceModels);

        // Act
        var result = await sut.Handle(command, new CancellationToken());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValidResponse, Is.EqualTo(true));
            Assert.That(result.Result, Is.InstanceOf<SuccessCommandResult>());
        });
    }
}
