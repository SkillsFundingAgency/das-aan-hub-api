using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Services;
public static class MemberPreferenceService
{
    public const int JobTitle = 1;
    public const int Biography = 2;
    public const int Apprenticeship = 3;
    public const int LinkedIn = 4;


    public static List<MemberPreference> GetDefaultPreferencesForMember(UserType userType, Guid memberId)
    {
        return userType switch
        {
            UserType.Employer => getDefaultPreferencesForMember(memberId),
            UserType.Apprentice => getDefaultPreferencesForMember(memberId),
            _ => throw new NotImplementedException()
        };
    }
    private static List<MemberPreference> getDefaultPreferencesForMember(Guid memberId)
    {
        return new List<MemberPreference>()
        {
            new MemberPreference {   MemberId = memberId, PreferenceId =  JobTitle, AllowSharing = false },
            new MemberPreference {   MemberId = memberId, PreferenceId =  Biography, AllowSharing = false },
            new MemberPreference {   MemberId = memberId, PreferenceId =  Apprenticeship, AllowSharing = true },
            new MemberPreference {   MemberId = memberId, PreferenceId =  LinkedIn, AllowSharing = false }
        };
    }
}
