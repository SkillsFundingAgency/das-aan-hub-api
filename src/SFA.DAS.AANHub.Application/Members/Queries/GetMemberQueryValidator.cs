using FluentValidation;

namespace SFA.DAS.AANHub.Application.Employers.Queries
{
    public class GetMemberQueryValidator : AbstractValidator<GetMemberQuery>
    {
        public const string UserRefMissingMessage = "UserRef must have a value";

        public GetMemberQueryValidator()
        {
            RuleFor(a => a.UserRef)
                .NotEmpty()
                .WithMessage(UserRefMissingMessage);
        }
    }
}