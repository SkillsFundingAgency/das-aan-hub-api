using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;

public record GetMemberProfilesWithPreferencesQuery(Guid MemberId, bool IsPublicView) : IRequest<ValidatedResponse<GetMemberProfilesWithPreferencesQueryResult>>;
