using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;
public class UpdateMemberProfilesCommand : IRequest<ValidatedResponse<SuccessCommandResult>>, IMemberId
{
    public Guid MemberId { get; set; }
    public IEnumerable<UpdateProfileModel> MemberProfiles { get; set; }
    public IEnumerable<UpdatePreferenceModel> MemberPreferences { get; set; }

    public UpdateMemberProfilesCommand(Guid memberId, IEnumerable<UpdateProfileModel> profiles, IEnumerable<UpdatePreferenceModel> preferences)
    {
        MemberId = memberId;
        MemberProfiles = profiles;
        MemberPreferences = preferences;
    }
}

public class UpdateProfileModel
{
    public int MemberProfileId { get; set; }
    public string? Value { get; set; } = null!;
}
public class UpdatePreferenceModel
{
    public int PreferenceId { get; set; }
    public bool Value { get; set; }
}
