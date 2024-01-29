using FluentValidation;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
public class GetMemberActivitiesQueryValidator : AbstractValidator<GetMemberActivitiesQuery>
{
    public const string MemberIdMissingMessage = "MemberId must have a value";

    public GetMemberActivitiesQueryValidator()
    {
        RuleFor(a => a.MemberId)
            .NotEmpty()
            .WithMessage(MemberIdMissingMessage);
    }
}