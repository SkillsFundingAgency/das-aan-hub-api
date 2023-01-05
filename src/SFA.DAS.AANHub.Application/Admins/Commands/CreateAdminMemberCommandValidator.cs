using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Admins.Commands
{
    public class CreateAdminMemberCommandValidator : AbstractValidator<CreateAdminMemberCommand>
    {
        public CreateAdminMemberCommandValidator(IRegionsReadRepository regionsReadRepository)
        {
            Include(new CreateMemberCommandBaseValidator(regionsReadRepository));
            RuleFor(c => c.UserName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(200);
        }
    }
}
