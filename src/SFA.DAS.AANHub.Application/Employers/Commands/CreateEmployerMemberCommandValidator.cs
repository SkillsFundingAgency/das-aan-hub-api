using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Employers.Commands
{
    public class CreateEmployerMemberCommandValidator : AbstractValidator<CreateEmployerMemberCommand>
    {
        public CreateEmployerMemberCommandValidator(IRegionsReadRepository regionsReadRepository)
        {
            Include(new CreateMemberCommandBaseValidator(regionsReadRepository));
            RuleFor(c => c.UserId)
                .NotEmpty();
            RuleFor(c => c.Organisation)
                .NotEmpty()
                .MaximumLength(250);
            RuleFor(c => c.AccountId)
                .NotEmpty();
            RuleFor(c => c.RequestedByUserId).NotEqual(Guid.Empty).NotNull();
        }

    }
}
