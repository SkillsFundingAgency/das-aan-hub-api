using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands
{
    public class CreateApprenticeMemberCommandValidator : AbstractValidator<CreateApprenticeMemberCommand>
    {
        public const string ApprenticeIdNotFoundMessage = "ApprenticeId was not found";

        public CreateApprenticeMemberCommandValidator(IMembersReadRepository memberReadRepository, IRegionsReadRepository regionsReadRepository, IApprenticesReadRepository apprenticesReadRepository)
        {
            Include(new CreateMemberCommandBaseValidator(regionsReadRepository));
            Include(new RequestedByMemberIdValidator(memberReadRepository));

            RuleFor(c => c.ApprenticeId)
            .NotEmpty()
            .MustAsync(async (ApprenticeId, cancellation) =>
            {
                var apprentice = await apprenticesReadRepository.GetApprentice(ApprenticeId);
                return apprentice == null;
            })
            .WithMessage(ApprenticeIdNotFoundMessage);
        }
    }
}
