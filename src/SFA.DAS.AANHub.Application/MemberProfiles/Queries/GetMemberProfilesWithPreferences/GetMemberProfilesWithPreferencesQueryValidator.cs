using FluentValidation;

namespace SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;

public class GetMemberProfilesWithPreferencesQueryValidator : AbstractValidator<GetMemberProfilesWithPreferencesQuery>
{
    public GetMemberProfilesWithPreferencesQueryValidator()
    {
        RuleFor(a => a.MemberId)
            .NotEmpty();
    }
}