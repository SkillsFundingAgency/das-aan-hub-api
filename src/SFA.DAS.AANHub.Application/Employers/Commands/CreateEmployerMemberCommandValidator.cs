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
                .MustAsync(async (command, cancellation) => await DoesUserAndAccountExist(command.AccountId, command.UserId, employersReadRepository))
                .WithMessage("UserId and AccountId pair already exist");
        }

        private static async Task<bool> DoesUserAndAccountExist(long accountId, long userId, IEmployersReadRepository employersReadRepository)
        {
            var result = await employersReadRepository.GetEmployerByAccountIdAndUserId(accountId, userId);
            return result == null;
        }
    }
}
