using FluentValidation;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberByEmail;

public class GetMemberByEmailQueryValidator : AbstractValidator<GetMemberByEmailQuery>
{
    public const string EmailMissingMessage = "Email must have a value";

    public GetMemberByEmailQueryValidator()
    {
        RuleFor(a => a.Email)
            .NotEmpty()
            .WithMessage(EmailMissingMessage);
    }
}