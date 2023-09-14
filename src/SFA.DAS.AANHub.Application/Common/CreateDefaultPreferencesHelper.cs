using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Common;
public class CreateDefaultPreferencesHelper
{
    private readonly IMemberPreferenceWriteRepository _memberPreferenceWriteRespository;
    public CreateDefaultPreferencesHelper(IMemberPreferenceWriteRepository memberPreferenceWriteRepository) => _memberPreferenceWriteRespository = memberPreferenceWriteRepository;
    public void CreateDefaultPreferences(Guid memberId)
    {
        var defaultMemberPreferences = Constants.DefaultMemberPreferencesAllowSharing.GetDefaultMemberPreferences();

        foreach (var defaultMemberPreference in defaultMemberPreferences)
        {
            _memberPreferenceWriteRespository.Create(new MemberPreference()
            {
                MemberId = memberId,
                PreferenceId = defaultMemberPreference.Id,
                AllowSharing = defaultMemberPreference.AllowSharing
            });
        }
    }
}