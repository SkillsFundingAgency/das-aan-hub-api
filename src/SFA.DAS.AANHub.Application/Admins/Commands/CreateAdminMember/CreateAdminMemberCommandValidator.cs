using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember
{
    public class CreateAdminMemberCommandValidator : AbstractValidator<CreateAdminMemberCommand>
    {
        public const string AdminAlreadyExistsErrorMessage = "Username already exists";

        public CreateAdminMemberCommandValidator(IRegionsReadRepository regionsReadRepository, IMembersReadRepository membersReadRepository,
            IAdminsReadRepository adminsReadRepository)
        {
            Include(new CreateMemberCommandBaseValidator(regionsReadRepository));
            Include(new RequestedByMemberIdValidator(membersReadRepository));
            RuleFor(c => c.UserName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(200)
                .MustAsync(async (userName, cancellation) =>
                {
                    var admin = await adminsReadRepository.GetAdminByUserName(userName);
                    return admin == null;
                })
                .WithMessage(AdminAlreadyExistsErrorMessage);
        }
    }
}