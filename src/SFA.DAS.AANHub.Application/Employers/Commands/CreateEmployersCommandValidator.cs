using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Domain.Enums;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Employers.Commands
{
    public class CreateEmployersCommandValidator : AbstractValidator<CreateEmployersCommand>
    {
        public CreateEmployersCommandValidator(IRegionsReadRepository regionsReadRepository)
        {
            Include(new BaseMemberValidator(regionsReadRepository));
            RuleFor(c => c.UserId)
                .NotEmpty();
            RuleFor(c => c.Organisation)
                .NotEmpty()
                .MaximumLength(250);
            RuleFor(c => c.AccountId)
                .NotEmpty();
            RuleFor(c => c.UserType)
                .NotEmpty()
                .Equal(MembershipUserType.Employer);
        }

    }
}
