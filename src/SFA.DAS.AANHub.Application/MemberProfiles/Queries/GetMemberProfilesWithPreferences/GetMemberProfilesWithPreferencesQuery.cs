using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;

public class GetMemberProfilesWithPreferencesQuery
    : IRequest<ValidatedResponse<GetMemberProfilesWithPreferencesQueryResult>>, IRequestedByMemberId, IMemberId
{
    public Guid RequestedByMemberId { get; set; }
    public Guid MemberId { get; set; }
    public bool IsPublicView { get; set; }
}
