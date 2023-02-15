using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Admins.Commands.PatchAdminMember
{
    public class PatchAdminMemberCommandValidator : AbstractValidator<PatchAdminMemberCommand>
    {
        private const string AdminDoesNotExistErrorMessage = "Username does not exist";

        private static readonly List<string> PatchFields =
            new()
            {
                "Email",
                "Name"
            };

        public PatchAdminMemberCommandValidator(IMembersReadRepository membersReadRepository, IAdminsReadRepository adminsReadRepository)
        {
            Include(new PatchMemberCommandBaseValidator<Admin>(PatchFields));
            Include(new RequestedByMemberIdValidator(membersReadRepository));

            RuleFor(c => c.UserName)
                .NotEmpty()
                .MaximumLength(200)
                .MustAsync(async (userName, _) =>
                {
                    var admin = await adminsReadRepository.GetAdminByUserName(userName);
                    return admin != null;
                })
                .WithMessage(AdminDoesNotExistErrorMessage);

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