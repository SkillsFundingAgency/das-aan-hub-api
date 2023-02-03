using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Employers.Commands
{
    public class CreateEmployerMemberCommandValidator : AbstractValidator<CreateEmployerMemberCommand>
    {
        private const string EmployerPairAlreadyCreatedErrorMessage = "UserRef already exists";

        private readonly IEmployersReadRepository _employersReadRepository;

        public CreateEmployerMemberCommandValidator(IRegionsReadRepository regionsReadRepository, IMembersReadRepository membersReadRepository,
            IEmployersReadRepository employersReadRepository)
        {
            _employersReadRepository = employersReadRepository;

            Include(new CreateMemberCommandBaseValidator(regionsReadRepository));
            Include(new RequestedByMemberIdValidator(membersReadRepository));
            RuleFor(c => c.UserRef)
                .NotEmpty()
                .MustAsync(async (command, _, _) => await CheckUserRefExists(command.UserRef))
                .WithMessage(EmployerPairAlreadyCreatedErrorMessage);

            RuleFor(c => c.Organisation)
                .NotEmpty()
                .MaximumLength(250);

            RuleFor(c => c.AccountId)
                .NotEmpty();
        }

        private async Task<bool> CheckUserRefExists(Guid userRef)
        {
            var result = await _employersReadRepository.GetEmployerByUserRef(userRef);
            return result == null;
        }
    }
}