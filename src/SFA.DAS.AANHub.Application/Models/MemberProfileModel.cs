using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Models;
public record MemberProfileModel(int ProfileId, string value, int PreferenceId)
{
    public static implicit operator MemberProfileModel(MemberProfile source) => new(source.ProfileId, source.ProfileValue, source.Profile.PreferenceId);
}
