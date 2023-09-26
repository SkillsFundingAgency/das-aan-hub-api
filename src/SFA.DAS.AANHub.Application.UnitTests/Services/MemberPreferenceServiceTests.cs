using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Services;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Services;

[TestFixture]
public class MemberPreferenceServiceTests
{
    readonly List<MemberPreference> _defaultMemberPreferences = new List<MemberPreference>()
    {
        new MemberPreference { PreferenceId =  MemberPreferenceService.JobTitle, AllowSharing = false },
        new MemberPreference { PreferenceId =  MemberPreferenceService.Biography, AllowSharing = false },
        new MemberPreference { PreferenceId =  MemberPreferenceService.Apprenticeship, AllowSharing = true },
        new MemberPreference { PreferenceId =  MemberPreferenceService.LinkedIn, AllowSharing = false }
    };

    [Test, RecursiveMoqInlineAutoData("Employer")]
    [RecursiveMoqInlineAutoData("Apprentice")]
    [RecursiveMoqInlineAutoData("None")]
    [RecursiveMoqInlineAutoData("Partner")]
    [RecursiveMoqInlineAutoData("Admin")]
    public void Service_MemberPreference_ReturnsDefaultPreferences(string userType, Guid memberId)
    {
        if (userType == "Employer" || userType == "Apprentice")
        {
            var result = MemberPreferenceService.GetDefaultPreferencesForMember(Enum.Parse<Domain.Common.UserType>(userType), memberId);
            using (new AssertionScope())
            {
                result.Should().BeOfType<List<MemberPreference>>();
                result.ForEach(result => result.MemberId.Should().Be(memberId));
                result.Count.Should().Be(_defaultMemberPreferences.Count);
            }
        }
        else
        {
            Action act = () => MemberPreferenceService.GetDefaultPreferencesForMember(Enum.Parse<Domain.Common.UserType>(userType), memberId);
            act.Should().Throw<Exception>();
        }
    }
}

