using FluentValidation;

namespace SFA.DAS.AANHub.Application.Partners.Queries
{
    public class GetPartnerMemberQueryValidator : AbstractValidator<GetPartnerMemberQuery>
    {
        public GetPartnerMemberQueryValidator()
        {
            RuleFor(a => a.UserName)
                .NotEmpty();
        }
    }
}