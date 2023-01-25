using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Employers.Queries
{
    public class GetEmployerMemberQueryValidator : AbstractValidator<GetEmployerMemberQuery>
    {
        public const string UserIdNotFoundMessage = "UserId was not found";
        public GetEmployerMemberQueryValidator(IEmployersReadRepository employersReadRepository)
        {
            RuleFor(a => a.UserId)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (userId, cancellation) =>
                {
                    var employer = await employersReadRepository.GetEmployerByUserId(userId);
                    return employer != null;
                })
                .WithMessage(UserIdNotFoundMessage);
        }
    }
}
