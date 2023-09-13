using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;
public class UpdateMemberProfilesCommand : IRequest<ValidatedResponse<SuccessCommandResult>>, IRequestedByMemberId
{
    public Guid MemberId { get; set; }
    public Guid RequestedByMemberId { get; set; }
    public IEnumerable<UpdateProfileModel> Profiles { get; set; }
    public IEnumerable<UpdatePreferenceModel> Preferences { get; set; }

    public UpdateMemberProfilesCommand(Guid memberId, Guid requestedByMemberId, IEnumerable<UpdateProfileModel> profiles, IEnumerable<UpdatePreferenceModel> preferences)
    {
        MemberId = memberId;
        RequestedByMemberId = requestedByMemberId;
        Profiles = profiles;
        Preferences = preferences;
    }
}


public class UpdateProfileModel
{
    public int ProfileId { get; set; }
    public string? Value { get; set; } = null!;
}
public class UpdatePreferenceModel
{
    public int PreferenceId { get; set; }
    public bool Value { get; set; }
}
