using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;
using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.MemberProfiles.Queries.GetMemberProfilesWithPreferences;
public class GetMemberProfilesWithPreferencesQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_WhenPublicViewIsFalse_Returns_All_MemberProfileWithPreference(
        [Frozen] Mock<IMemberProfilesReadRepository> memberProfilesReadRepository,
        [Frozen] Mock<IMemberPreferencesReadRepository> memberPreferencesReadRepository,
        Guid memberId)
    {
        bool isPublicView = false;

        List<MemberProfile> memberProfiles = new()
        {
            new MemberProfile { ProfileId= 41, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= 1 } },
            new MemberProfile { ProfileId= 42, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= 1 } },
            new MemberProfile { ProfileId= 43, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= 2 } },
            new MemberProfile { ProfileId= 44, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= null } },
        };

        List<MemberPreference> memberPreferences = new()
        {
            new MemberPreference{ PreferenceId = 1, AllowSharing =  true },
            new MemberPreference{ PreferenceId = 2, AllowSharing =  true },
            new MemberPreference{ PreferenceId = 3, AllowSharing =  false },
        };

        memberProfilesReadRepository.Setup(a => a.GetMemberProfilesByMember(memberId, new CancellationToken())).ReturnsAsync(memberProfiles);
        memberPreferencesReadRepository.Setup(a => a.GetMemberPreferencesByMember(memberId, new CancellationToken())).ReturnsAsync(memberPreferences);

        GetMemberProfilesWithPreferencesQueryHandler sut = new(memberProfilesReadRepository.Object, memberPreferencesReadRepository.Object);
        var result = await sut.Handle(new GetMemberProfilesWithPreferencesQuery(memberId, isPublicView), new CancellationToken());

        GetMemberProfilesWithPreferencesQueryResult queryOutPut = new();

        queryOutPut.Profiles = memberProfiles.Select(p => (MemberProfileModel)p);
        queryOutPut.Preferences = memberPreferences.Select(p => (MemberPreferenceModel)p);

        result.Result.Should().BeEquivalentTo(queryOutPut);
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenPublicViewIsTrue_Returns_MemberProfilesWherePreferenceSharingIsAllowed(
        [Frozen] Mock<IMemberProfilesReadRepository> memberProfilesReadRepository,
        [Frozen] Mock<IMemberPreferencesReadRepository> memberPreferencesReadRepository,
        Guid memberId)
    {
        bool isPublicView = true;

        List<MemberProfile> memberProfiles = new()
        {
            new MemberProfile { ProfileId= 41, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= 1 } },
            new MemberProfile { ProfileId= 42, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= 1 } },
            new MemberProfile { ProfileId= 43, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= 2 } },
            new MemberProfile { ProfileId= 44, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= 3 } },
            new MemberProfile { ProfileId= 45, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= null } },
        };

        List<MemberPreference> memberPreferences = new()
        {
            new MemberPreference{ PreferenceId = 1, AllowSharing =  true },
            new MemberPreference{ PreferenceId = 2, AllowSharing =  true },
            new MemberPreference{ PreferenceId = 3, AllowSharing =  false },
        };

        memberProfilesReadRepository.Setup(a => a.GetMemberProfilesByMember(memberId, new CancellationToken())).ReturnsAsync(memberProfiles);
        memberPreferencesReadRepository.Setup(a => a.GetMemberPreferencesByMember(memberId, new CancellationToken())).ReturnsAsync(memberPreferences);

        GetMemberProfilesWithPreferencesQueryHandler sut = new(memberProfilesReadRepository.Object, memberPreferencesReadRepository.Object);
        var result = await sut.Handle(new GetMemberProfilesWithPreferencesQuery(memberId, isPublicView), new CancellationToken());

        GetMemberProfilesWithPreferencesQueryResult queryOutPut = new();
        List<MemberProfileModel> profilesResult = new();

        var preferenceIdsAllowedForSharing = memberPreferences.Where(x => x.AllowSharing).Select(x => x.PreferenceId);


        foreach (var profileModel in memberProfiles!.Select(p => (MemberProfileModel)p))
        {
            if (profileModel.PreferenceId.HasValue && preferenceIdsAllowedForSharing.Contains((int)profileModel.PreferenceId))
                profilesResult.Add(profileModel);
        }

        queryOutPut.Profiles = profilesResult;
        queryOutPut.Preferences = null!;

        result.Result.Should().BeEquivalentTo(queryOutPut);
    }
}
