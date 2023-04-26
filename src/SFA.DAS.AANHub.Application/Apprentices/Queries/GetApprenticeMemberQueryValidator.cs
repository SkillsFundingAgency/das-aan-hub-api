using FluentValidation;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries;

public class GetApprenticeMemberQueryValidator : AbstractValidator<GetApprenticeMemberQuery>
{
    public GetApprenticeMemberQueryValidator()
    {
        RuleFor(a => a.ApprenticeId)
            .NotEmpty();
    }
}