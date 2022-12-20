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
                .MaximumLength(250);
            RuleFor(c => c.Information)
                .NotEmpty()
                .MaximumLength(250);
            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(256)
                .Matches(Constants.RegularExpressions.EmailRegex);
            RuleFor(x => x.Joined)
                .NotEmpty();
            RuleFor(x => x.Regions).MustAsync(async (regions, cancellationToken) =>
            {
                if (regions == null) return true;

                var dbRegions = await regionsReadRepository.GetAllRegions();
                var regionList = dbRegions.Select(item => item.Id).ToList();

                return regions.All(region => regionList.Contains(region));

            }).WithMessage("Region value must be in range");

            RuleFor(x => x.UserType)
                .NotEmpty();
        }

    }
}
