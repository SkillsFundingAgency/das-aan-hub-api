using FluentValidation;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMember;

public class GetMemberQueryValidator : AbstractValidator<GetMemberQuery>
{
    public const string MemberIdMissingMessage = "MemberId must have a value";

    public GetMemberQueryValidator()
    {
        RuleFor(a => a.MemberId)
            .NotEmpty()
            .WithMessage(MemberIdMissingMessage);
    }
}