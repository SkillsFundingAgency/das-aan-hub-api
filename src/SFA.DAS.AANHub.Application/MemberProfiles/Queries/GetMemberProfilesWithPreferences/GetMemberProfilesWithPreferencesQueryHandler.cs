using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;
public class GetMemberProfilesWithPreferencesQueryHandler : IRequestHandler<GetMemberProfilesWithPreferencesQuery, ValidatedResponse<GetMemberProfilesWithPreferencesQueryResult>>
{
    private readonly IMemberProfilesReadRepository _memberProfilesReadRepository;
    private readonly IMemberPreferencesReadRepository _memberPreferencesReadRepository;

    public GetMemberProfilesWithPreferencesQueryHandler(IMemberProfilesReadRepository memberProfilesReadRepository, IMemberPreferencesReadRepository memberPreferencesReadRepository)
    {
        _memberProfilesReadRepository = memberProfilesReadRepository;
        _memberPreferencesReadRepository = memberPreferencesReadRepository;
    }

    public async Task<ValidatedResponse<GetMemberProfilesWithPreferencesQueryResult>> Handle(GetMemberProfilesWithPreferencesQuery request, CancellationToken cancellationToken)
    {
        var memberProfiles = (await _memberProfilesReadRepository.GetMemberProfilesByMember(request.MemberId, cancellationToken)).Select(p => (MemberProfileModel)p);
        var memberPreferences = await _memberPreferencesReadRepository.GetMemberPreferencesByMember(request.MemberId, cancellationToken);

        var preferenceIdsAllowedForSharing = memberPreferences.Where(x => x.AllowSharing).Select(x => x.PreferenceId);

        GetMemberProfilesWithPreferencesQueryResult result = new();

        if (request.IsPublicView)
        {
            List<MemberProfileModel> profilesResult = new();
            foreach (var m in memberProfiles)
            {
                if (m.PreferenceId.HasValue && preferenceIdsAllowedForSharing.Contains((int)m.PreferenceId))
                    profilesResult.Add(m);
            }

            result.Profiles = profilesResult;
            result.Preferences = null!;
        }
        else
        {
            result.Profiles = memberProfiles;
            result.Preferences = memberPreferences.Select(p => (MemberPreferenceModel)p);
        }

        return (memberProfiles != null && memberPreferences != null)
            ? new ValidatedResponse<GetMemberProfilesWithPreferencesQueryResult>(result)
            : ValidatedResponse<GetMemberProfilesWithPreferencesQueryResult>.EmptySuccessResponse();
    }
}
