using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;

public class GetMemberProfilesWithPreferencesQueryValidator : AbstractValidator<GetMemberProfilesWithPreferencesQuery>
{
    public GetMemberProfilesWithPreferencesQueryValidator(IMembersReadRepository membersReadRepository)
    {
        Include(new MemberIdValidator(membersReadRepository));
    }
}