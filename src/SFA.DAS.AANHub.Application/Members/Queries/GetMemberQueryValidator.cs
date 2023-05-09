using FluentValidation;

namespace SFA.DAS.AANHub.Application.Employers.Queries
{
    public class GetMemberQueryValidator : AbstractValidator<GetMemberQuery>
    {
        public GetMemberQueryValidator()
        {
            RuleFor(a => a.UserRef)
                .NotEmpty();
        }
    }
}