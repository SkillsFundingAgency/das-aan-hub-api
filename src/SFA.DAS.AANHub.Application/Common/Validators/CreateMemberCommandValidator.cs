using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Commands;

namespace SFA.DAS.AANHub.Application.Common.Validators
{
    public class CreateMemberCommandValidator : AbstractValidator<BaseMemberCommand>
    {
        public CreateMemberCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(250);
            RuleFor(c => c.Information)
                .NotEmpty()
                .MaximumLength(250);
            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(256)
                .Matches(Constants.RegularExpressions.EmailRegex);
        }

    }
}
