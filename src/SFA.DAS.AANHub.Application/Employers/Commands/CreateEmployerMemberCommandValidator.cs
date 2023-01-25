using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Employers.Commands
{
    public class CreateEmployerMemberCommandValidator : AbstractValidator<CreateEmployerMemberCommand>
    {
        private const string EmployerPairAlreadyCreatedErrorMessage = "UserId and AccountId pair already exist";

        private readonly IEmployersReadRepository _employersReadRepository;

        public CreateEmployerMemberCommandValidator(IRegionsReadRepository regionsReadRepository, IMembersReadRepository membersReadRepository,
            IEmployersReadRepository employersReadRepository)
        {
            _employersReadRepository = employersReadRepository;

            Include(new CreateMemberCommandBaseValidator(regionsReadRepository));
            Include(new RequestedByMemberIdValidator(membersReadRepository));
            RuleFor(c => c.UserId)
                .NotEmpty();

            RuleFor(c => c.Organisation)
                .NotEmpty()
                .MaximumLength(250);

            RuleFor(c => c.AccountId)
                .NotEmpty()
                .MustAsync(async (command, _, _) => await CheckAccountIdAndUserIdPair(command.AccountId, command.UserId))
                .WithMessage(EmployerPairAlreadyCreatedErrorMessage);

            RuleFor(c => c.UserId)
                .NotEmpty()
                .MustAsync(async (command, _, _) => await CheckAccountIdAndUserIdPair(command.AccountId, command.UserId))
                .WithMessage(EmployerPairAlreadyCreatedErrorMessage);
        }

        private async Task<bool> CheckAccountIdAndUserIdPair(long accountId, long userId)
        {
            var result = await _employersReadRepository.GetEmployerByAccountIdAndUserId(accountId, userId);
            return result == null;
        }
    }
}