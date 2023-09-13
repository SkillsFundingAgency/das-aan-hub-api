using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Services;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Services;

[TestFixture]
public class MemberPreferenceServiceTests
{
    readonly List<MemberPreference> _defaultMemberPreferences = new List<MemberPreference>()
    {
        new MemberPreference { PreferenceId =  MemberPreferenceService.AreasOfInterest, AllowSharing = true },
        new MemberPreference { PreferenceId =  MemberPreferenceService.JobTitle, AllowSharing = true },
        new MemberPreference { PreferenceId =  MemberPreferenceService.EmployerName, AllowSharing = true },
        new MemberPreference { PreferenceId =  MemberPreferenceService.RegionWhereYouWork, AllowSharing = true },
        new MemberPreference { PreferenceId =  MemberPreferenceService.Apprenticeship, AllowSharing = true },
        new MemberPreference { PreferenceId =  MemberPreferenceService.RegionWhereYouLive, AllowSharing = true },
        new MemberPreference { PreferenceId =  MemberPreferenceService.Biography, AllowSharing = true },

        new MemberPreference { PreferenceId =  MemberPreferenceService.Sector, AllowSharing = false },
        new MemberPreference { PreferenceId =  MemberPreferenceService.EmployerAddress, AllowSharing = false },
        new MemberPreference { PreferenceId =  MemberPreferenceService.LinkedIn, AllowSharing = false }
    };


    [TestCase("Employer")]
    [TestCase("Apprentice")]
    [TestCase("None")]
    [TestCase("Partner")]
    [TestCase("Admin")]
    public void Service_MemberPreference_ReturnsDefaultPreferences(string userType)
    {
        if (userType == "Employer" || userType == "Apprentice")
        {
            var result = MemberPreferenceService.GetDefaultMemberPreferences(Enum.Parse<Domain.Common.UserType>(userType));
            result.Should().BeEquivalentTo(_defaultMemberPreferences);
        }
        else
        {
            Action act = () => MemberPreferenceService.GetDefaultMemberPreferences(Enum.Parse<Domain.Common.UserType>(userType));
            act.Should().Throw<Exception>();
        }
    }
}

