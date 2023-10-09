using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.MemberProfiles.Queries.GetMemberProfilesWithPreferences;
public class GetMemberProfilesWithPreferencesQueryHandlerTests
{
    Mock<IMemberProfilesReadRepository> _memberProfilesReadRepository = null!;
    Mock<IMemberPreferencesReadRepository> _memberPreferencesReadRepository = null!;
    GetMemberProfilesWithPreferencesQueryHandler _sut = null!;
    ValidatedResponse<GetMemberProfilesWithPreferencesQueryResult> _response = null!;
    Guid memberId = Guid.NewGuid();
    Guid requestedByMemberId = Guid.NewGuid();

    static readonly List<MemberProfile> memberProfiles = new()
        {
            new MemberProfile { ProfileId= 41, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= 1 } },
            new MemberProfile { ProfileId= 42, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= 1 } },
            new MemberProfile { ProfileId= 43, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= 2 } },
            new MemberProfile { ProfileId= 44, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= 3 } },
            new MemberProfile { ProfileId= 45, ProfileValue = Guid.NewGuid().ToString(), Profile = new Profile(){ PreferenceId= null } },
        };

    static readonly List<MemberPreference> memberPreferences = new()
        {
            new MemberPreference{ PreferenceId = 1, AllowSharing =  true },
            new MemberPreference{ PreferenceId = 2, AllowSharing =  true },
            new MemberPreference{ PreferenceId = 3, AllowSharing =  false },
        };

    [SetUp]
    public void Init()
    {
        //Arranging
        _memberProfilesReadRepository = new();
        _memberPreferencesReadRepository = new();

        _memberProfilesReadRepository.Setup(a => a.GetMemberProfilesByMember(memberId, new CancellationToken())).ReturnsAsync(memberProfiles);
        _memberPreferencesReadRepository.Setup(a => a.GetMemberPreferencesByMember(memberId, new CancellationToken())).ReturnsAsync(memberPreferences);

        _sut = new(_memberProfilesReadRepository.Object, _memberPreferencesReadRepository.Object);
    }

    //Action
    private async Task InvokeHandler(bool isPublicView)
    {
        _response = await _sut.Handle(new GetMemberProfilesWithPreferencesQuery() { RequestedByMemberId = requestedByMemberId, MemberId = memberId, IsPublicView = isPublicView }, new CancellationToken());
    }

    [Test]
    public async Task Handle_PublicViewIsTrue_HasPreferenceToShare_ReturnsProfileAllowedToShare()
    {
        await InvokeHandler(true);
        _response.Result.Profiles.Select(x => x.ProfileId).Should().Contain(new List<int> { 41, 42, 43 });
    }

    [Test]
    public async Task Handle_PublicViewIsTrue_ProfileDoesNotHavePreference_ReturnsProfile()
    {
        await InvokeHandler(true);
        _response.Result.Profiles.Select(x => x.ProfileId).Should().Contain(45);
    }

    [Test]
    public async Task Handle_PublicViewIsTrue_HasNoPreferenceToShare_IgnoresProfile()
    {
        await InvokeHandler(true);
        _response.Result.Profiles.Select(x => x.ProfileId).Should().NotContain(44);
    }

    [Test]
    public async Task Handle_PubliViewFalse_ReturnsAllProfiles()
    {
        await InvokeHandler(false);
        using (new AssertionScope("Public view false should return all profiles"))
        {
            _response.Result.Profiles.Count().Should().Be(5);
            _response.Result.Profiles.Select(x => x.ProfileId).Should().Contain(new List<int> { 41, 42, 43, 44, 45 });
        }
    }

    [TestCase(true)]
    [TestCase(false)]
    [TestCase(null)]
    public async Task Handle_ReturnsAllPreferences(bool @public = true)
    {
        await InvokeHandler(@public);
        using (new AssertionScope("Handle should return all preferences"))
        {
            _response.Result.Preferences.Count().Should().Be(3);
            _response.Result.Preferences.Select(x => x.PreferenceId).Should().Contain(new List<int> { 1, 2, 3 });
        }
    }

    [TearDown]
    public void Dispose()
    {
        _sut = null!;
        _response = null!;
    }
}
