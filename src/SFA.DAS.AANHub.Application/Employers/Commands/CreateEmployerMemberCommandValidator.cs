using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Employers.Commands
{
    public class CreateEmployerMemberCommandValidator : AbstractValidator<CreateEmployerMemberCommand>
    {
        public CreateEmployerMemberCommandValidator(IRegionsReadRepository regionsReadRepository, IMembersReadRepository membersReadRepository, IEmployersReadRepository employersReadRepository)
        {
            Include(new CreateMemberCommandBaseValidator(regionsReadRepository));
            Include(new RequestedByMemberIdValidator(membersReadRepository));
            RuleFor(c => c.UserId)
                .NotEmpty();
            RuleFor(c => c.Organisation)
                .NotEmpty()
                .MaximumLength(250);
            RuleFor(c => c.AccountId)
                .NotEmpty();
            RuleFor(c => c)
                .NotEmpty()
                .MustAsync(async (command, cancellation) =>
                {
                    var result = await employersReadRepository.GetEmployerByAccountIdAndUserId(command.AccountId, command.UserId);
                    return result == null;
                })
                .WithMessage("UserId and AccountId pair already exist");
        }
    }
}
