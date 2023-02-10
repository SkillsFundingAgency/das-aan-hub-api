using FluentValidation;

namespace SFA.DAS.AANHub.Application.Employers.Queries
{
    public class GetEmployerMemberQueryValidator : AbstractValidator<GetEmployerMemberQuery>
    {
        public GetEmployerMemberQueryValidator()
        {
            RuleFor(a => a.UserRef)
                .NotEmpty();
        }
    }
}