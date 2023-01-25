using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Apprentices.Queries
{
    public class GetApprenticeMemberQueryValidator : AbstractValidator<GetApprenticeMemberQuery>
    {
        public GetApprenticeMemberQueryValidator(IApprenticesReadRepository apprenticesReadRepository)
        {
            RuleFor(a => a.ApprenticeId)
                .NotNull()
                .NotEmpty();
        }
    }
}