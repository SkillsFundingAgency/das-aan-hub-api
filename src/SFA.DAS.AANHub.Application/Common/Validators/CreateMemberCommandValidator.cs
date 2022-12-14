using FluentValidation;
using SFA.DAS.AANHub.Application.Commands.CreateMember;

namespace SFA.DAS.AANHub.Application.Common.Validators
{
    public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
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
            RuleFor(x => x.Joined)
                .NotEmpty();
            RuleFor(x => x.Regions).Custom((regions, context) =>
            {
                if (regions == null) return;
                foreach (var region in regions)
                {
                    if (region is < 0 or > 9)
                    {
                        context.AddFailure("Region value must be in range");
                    }
                }
            });
            RuleFor(x => x.Id)
                .NotEmpty()
                .MaximumLength(250);
            RuleFor(x => x.Organisation)
                .NotEmpty()
                .MaximumLength(250);
            RuleFor(x => x.UserType)
                .NotEmpty();
        }

    }
}
