using FluentValidation;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesQueryValidator : AbstractValidator<GetProfilesQuery>
    {
        public GetProfilesQueryValidator()
        {
            RuleFor(a => a.UserType)
                .NotEmpty()
                .IsEnumName(typeof(UserType), caseSensitive: false);
        }
    }
}
