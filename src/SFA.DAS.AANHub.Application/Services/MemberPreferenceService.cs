using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Services;
public static class MemberPreferenceService
{
    public const int AreasOfInterest = 1;
    public const int JobTitle = 2;
    public const int EmployerName = 3;
    public const int EmployerAddress = 4;
    public const int RegionWhereYouWork = 5;
    public const int Apprenticeship = 6;
    public const int Sector = 7;
    public const int RegionWhereYouLive = 8;
    public const int Biography = 9;
    public const int LinkedIn = 10;

    public static List<MemberPreference> GetDefaultMemberPreferences(UserType userType)
    {
        return userType switch
        {
            UserType.Employer => GetDefaultMemberPreferences(),
            UserType.Apprentice => GetDefaultMemberPreferences(),
            _ => throw new NotImplementedException()
        };
    }
    private static List<MemberPreference> GetDefaultMemberPreferences()
    {
        return new List<MemberPreference>()
        {
            new MemberPreference {   PreferenceId =  AreasOfInterest, AllowSharing = true },
            new MemberPreference {   PreferenceId =  JobTitle, AllowSharing = true },
            new MemberPreference {   PreferenceId =  EmployerName, AllowSharing = true },
            new MemberPreference {   PreferenceId =  RegionWhereYouWork, AllowSharing = true },
            new MemberPreference {   PreferenceId =  Apprenticeship, AllowSharing = true },
            new MemberPreference {   PreferenceId =  RegionWhereYouLive, AllowSharing = true },
            new MemberPreference {   PreferenceId =  Biography, AllowSharing = true },

            new MemberPreference {   PreferenceId =  Sector, AllowSharing = false },
            new MemberPreference {   PreferenceId =  EmployerAddress, AllowSharing = false },
            new MemberPreference {   PreferenceId =  LinkedIn, AllowSharing = false }
        };
    }
}
