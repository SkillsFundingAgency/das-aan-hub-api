using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Partners.Commands.PatchPartnerMember
{
    public class PatchPartnerMemberCommandValidator : AbstractValidator<PatchPartnerMemberCommand>
    {
        private static readonly List<string> PatchFields =
            new()
            {
                "Email",
                "Name",
                "Organisation"
            };

        public PatchPartnerMemberCommandValidator(IMembersReadRepository membersReadRepository)
        {
            Include(new PatchMemberCommandBaseValidator<Partner>(PatchFields));
            Include(new RequestedByMemberIdValidator(membersReadRepository));

            RuleFor(x => x.UserName)
                .NotEmpty();

            When(x => x.IsPresentEmail,
                () =>
                {
                    RuleFor(x => x.Email)
                        .NotEmpty()
                        .MaximumLength(256)
                        .Matches(Constants.RegularExpressions.EmailRegex);
                });

            When(x => x.IsPresentName,
                () =>
                {
                    RuleFor(c => c.Name)
                        .NotEmpty()
                        .MaximumLength(200);
                });

            When(x => x.IsPresentOrganisation, 
                () =>
                {
                    RuleFor(c => c.Organisation)
                        .NotEmpty()
                        .MaximumLength(200);
                });
        }
    }
}
