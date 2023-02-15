using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember
{
    public class PatchApprenticeMemberCommandValidator : AbstractValidator<PatchApprenticeMemberCommand>
    {
        private static readonly List<string> PatchFields =
            new()
            {
                "Email",
                "Name"
            };

        public PatchApprenticeMemberCommandValidator(IMembersReadRepository membersReadRepository)
        {
            Include(new PatchMemberCommandBaseValidator<Apprentice>(PatchFields));
            Include(new RequestedByMemberIdValidator(membersReadRepository));

            RuleFor(x => x.ApprenticeId)
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
        }
    }
}