using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Common.Validators
{
    public class CreateMemberCommandBaseValidator : AbstractValidator<CreateMemberCommandBase>
    {
        public CreateMemberCommandBaseValidator(IRegionsReadRepository regionsReadRepository)
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(200);
            RuleFor(c => c.Information)
                .NotEmpty()
                .MaximumLength(250);
            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(256)
                .Matches(Constants.RegularExpressions.EmailRegex);
            RuleFor(x => x.Joined)
                .NotEmpty();
            RuleFor(x => x.RegionId).MustAsync(async (regionId, cancellationToken) =>
            {
                if (regionId == null) return true;

                var dbRegions = await regionsReadRepository.GetAllRegions();

                return dbRegions.Select(r => r.Id).Any(id => id == regionId);

            }).WithMessage("Region value must be in range");
        }

    }
}
