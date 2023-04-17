using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember
{
    public class CreateApprenticeMemberCommandValidator : AbstractValidator<CreateApprenticeMemberCommand>
    {
        public const string ApprenticeAlreadyExistsErrorMessage = "ApprenticeId already exists";

        public CreateApprenticeMemberCommandValidator(IRegionsReadRepository regionsReadRepository,
            IApprenticesReadRepository apprenticesReadRepository)
        {
            Include(new CreateMemberCommandBaseValidator(regionsReadRepository));

            RuleFor(c => c.ApprenticeId)
                .NotEmpty()
                .MustAsync(async (apprenticeId, cancellation) =>
                {
                    var apprentice = await apprenticesReadRepository.GetApprentice(apprenticeId);
                    return apprentice == null;
                })
                .WithMessage(ApprenticeAlreadyExistsErrorMessage);
        }
    }
}