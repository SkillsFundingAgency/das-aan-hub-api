using FluentValidation;

namespace SFA.DAS.AANHub.Application.Employers.Queries
{
    public class GetMemberQueryValidator : AbstractValidator<GetEmployerMemberQuery>
    {
        public GetMemberQueryValidator()
        {
            RuleFor(a => a.UserRef)
                .NotEmpty();
        }
    }
}