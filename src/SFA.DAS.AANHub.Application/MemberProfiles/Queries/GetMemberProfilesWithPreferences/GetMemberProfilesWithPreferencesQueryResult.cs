using SFA.DAS.AANHub.Application.Models;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;

public class GetMemberProfilesWithPreferencesQueryResult
{
    public IEnumerable<MemberProfileModel> Profiles { get; set; } = Enumerable.Empty<MemberProfileModel>();
    public IEnumerable<MemberPreferenceModel> Preferences { get; set; } = Enumerable.Empty<MemberPreferenceModel>();
}
