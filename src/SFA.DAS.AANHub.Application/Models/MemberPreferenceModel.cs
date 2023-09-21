using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Models;

public record MemberPreferenceModel(int PreferenceId, bool Value)
{
    public static implicit operator MemberPreferenceModel(MemberPreference source) => new(source.PreferenceId, source.AllowSharing);
}
