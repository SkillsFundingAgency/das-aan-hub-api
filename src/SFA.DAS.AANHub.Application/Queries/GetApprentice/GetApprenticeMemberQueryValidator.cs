using FluentValidation;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Queries.GetApprentice
{
    public class GetApprenticeMemberQueryValidator : AbstractValidator<GetApprenticeMemberQuery>
    {
        public const string ApprenticeIdNotFoundMessage = "ApprenticeId was not found";
        public GetApprenticeMemberQueryValidator(IApprenticesReadRepository apprenticesReadRepository)
        {
            RuleFor(a => a.ApprenticeId)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (apprenticeid, cancellation) =>
                {
                    var apprentice = await apprenticesReadRepository.GetApprentice(apprenticeid);
                    return apprentice == null;
                })
                .WithMessage(ApprenticeIdNotFoundMessage);
        }
    }
}
