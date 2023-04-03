using FluentValidation;

namespace SFA.DAS.AANHub.Application.StagedApprentices.Queries
{
    public class GetStagedApprenticeQueryValidator : AbstractValidator<GetStagedApprenticeQuery>
    {
        public GetStagedApprenticeQueryValidator()
        {
            RuleFor(a => a.LastName)
                .NotEmpty();

            RuleFor(a => a.DateOfBirth)
            .NotEmpty();

            RuleFor(a => a.Email)
                .NotEmpty()
                .MaximumLength(256)
                .Matches(Constants.RegularExpressions.EmailRegex);
        }
    }
}