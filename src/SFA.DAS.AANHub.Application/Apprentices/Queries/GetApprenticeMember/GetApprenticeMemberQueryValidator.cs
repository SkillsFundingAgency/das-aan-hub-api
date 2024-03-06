using FluentValidation;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries.GetApprenticeMember;

public class GetApprenticeMemberQueryValidator : AbstractValidator<GetApprenticeMemberQuery>
{
    public GetApprenticeMemberQueryValidator()
    {
        RuleFor(a => a.ApprenticeId)
            .NotEmpty();
    }
}