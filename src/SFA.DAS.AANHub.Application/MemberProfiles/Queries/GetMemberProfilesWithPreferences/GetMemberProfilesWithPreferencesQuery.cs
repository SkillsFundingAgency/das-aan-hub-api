using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;

public class GetMemberProfilesWithPreferencesQuery
    : IRequest<ValidatedResponse<GetMemberProfilesWithPreferencesQueryResult>>, IMemberId
{
    public Guid MemberId { get; set; }
    public bool IsPublicView { get; set; }
}
