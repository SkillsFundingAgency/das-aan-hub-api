using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Common;
public class CreateDefaultPreferencesHelper
{
    private readonly IMemberPreferenceWriteRepository _memberPreferenceWriteRespository;
    public CreateDefaultPreferencesHelper(IMemberPreferenceWriteRepository memberPreferenceWriteRepository) => _memberPreferenceWriteRespository = memberPreferenceWriteRepository;
    public void CreateDefaultPreferences(Guid memberId)
    {
        var memberPreferences = new List<MemberPreference>()
        {
            new MemberPreference()
            {
                MemberId = memberId,
                PreferenceId = 1,
                AllowSharing = false
            },
            new MemberPreference()
            {
                MemberId = memberId,
                PreferenceId = 2,
                AllowSharing = false
            },
            new MemberPreference()
            {
                MemberId = memberId,
                PreferenceId = 3,
                AllowSharing = true
            },
            new MemberPreference()
            {
                MemberId = memberId,
                PreferenceId = 4,
                AllowSharing = false
            }
        };

        foreach (var memberPreference in memberPreferences)
        {
            _memberPreferenceWriteRespository.Create(memberPreference);
        }
    }
}
