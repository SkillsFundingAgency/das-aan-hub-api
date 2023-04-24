using FluentValidation;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Admins.Commands.CreateAdminMember;

public class CreateAdminMemberCommandValidator : AbstractValidator<CreateAdminMemberCommand>
{
    public const string AdminAlreadyExistsErrorMessage = "Username already exists";

    public CreateAdminMemberCommandValidator(IAdminsReadRepository adminsReadRepository)
    {
        Include(new CreateMemberCommandBaseValidator());
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