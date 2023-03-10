using FluentValidation;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesByUserTypeQueryValidator : AbstractValidator<GetProfilesByUserTypeQuery>
    {
        public GetProfilesByUserTypeQueryValidator()
        {
            RuleFor(a => a.UserType)
                .NotEmpty()
                .IsEnumName(typeof(UserType), caseSensitive: false);
        }
    }
}
