using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Partners
{
    public class CreatePartnerMemberCommandValidator : AbstractValidator<CreatePartnerMemberCommand>
    {
        public const string PartnerAlreadyExistsErrorMessage = "Username already exists";

        public CreatePartnerMemberCommandValidator(IRegionsReadRepository regionsReadRepository, IMembersReadRepository membersReadRepository,
            IPartnersReadRepository partnersReadRepository)
        {
            Include(new CreateMemberCommandBaseValidator(regionsReadRepository));
            Include(new RequestedByMemberIdValidator(membersReadRepository));
            RuleFor(c => c.UserName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(200)
                .MustAsync(async (userName, cancellation) =>
                {
                    var partner = await partnersReadRepository.GetPartnerByUserName(userName);
                    return partner == null;
                })
                .WithMessage(PartnerAlreadyExistsErrorMessage);
            RuleFor(c => c.Organisation)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}