using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands
{
    public class CreateApprenticeMemberCommandValidator : AbstractValidator<CreateApprenticeMemberCommand>
    {
        public CreateApprenticeMemberCommandValidator(IMembersReadRepository memberReadRepository, IRegionsReadRepository regionsReadRepository)
        {
            Include(new CreateMemberCommandBaseValidator(regionsReadRepository));
            Include(new RequestedByMemberIdValidator(memberReadRepository));

            RuleFor(c => c.ApprenticeId)
            .NotEmpty();
        }
    }
}
